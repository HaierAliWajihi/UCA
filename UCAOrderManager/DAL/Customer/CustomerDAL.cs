using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCAOrderManager.Models.Customer;
using UCAOrderManager.Models.Template;

namespace UCAOrderManager.DAL.Customer
{
    public class CustomerDAL
    {
        public List<CustomerListViewModel> GetList()
        {
            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                int CustomerRoleID = (int)Models.Users.eUserRoleID.Customer;

                return (from r in db.tblUsers
                        where r.UserRoleID == CustomerRoleID
                        orderby r.ContactName, r.BusinessName
                        select new CustomerListViewModel()
                        {
                            UserID = r.UserID,
                            BusinessName = r.BusinessName,
                            ContactName = r.ContactName,
                            EMailID = r.EmailID,
                            Address = r.Address,
                            City = r.City,
                            Postcode = r.PostCode,
                            Country = r.Country,
                            IsApproved = r.IsApproved
                        }).ToList();
            }
        }

        public CustomerViewModel FindByID(int ID)
        {
            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                var r = db.tblUsers.Find(ID);
                if (r == null)
                {
                    return null;
                }

                return new CustomerViewModel()
                {
                    UserID = r.UserID,
                    BusinessName = r.BusinessName,
                    ContactName = r.ContactName,
                    EMailID = r.EmailID,
                    Address = r.Address,
                    City = r.City,
                    Postcode = r.PostCode,
                    Country = r.Country,
                    IntPhoneNo = r.IntPhoneNo,
                    AirportDestCity = r.AirportDestCity,
                    IsApproved = r.IsApproved
                };
            }
        }

        public CustomerViewModel FindByIDGetListViewModel(int ID)
        {
            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                return (from r in db.tblUsers
                        where r.UserID == ID

                        select new CustomerViewModel()
                        {
                            UserID = r.UserID,
                            BusinessName = r.BusinessName,
                            ContactName = r.ContactName,
                            EMailID = r.EmailID,
                            Address = r.Address,
                            City = r.City,
                            Postcode = r.PostCode,
                            Country = r.Country,
                            IsApproved = r.IsApproved
                        }).FirstOrDefault();
            }
        }

        public SavingResult SaveRecord(CustomerViewModel ViewModel)
        {
            SavingResult res = new SavingResult();

            if (ViewModel == null)
            {
                res.ExecutionResult = eExecutionResult.ValidationError;
                res.ValidationError = "information not passed to save.";
                return res;
            }
            if (ViewModel.EMailID == "")
            {
                res.ExecutionResult = eExecutionResult.ValidationError;
                res.ValidationError = "Email id is required";
                return res;
            }
            else if (ViewModel.BusinessName == "")
            {
                res.ExecutionResult = eExecutionResult.ValidationError;
                res.ValidationError = "Business Name is required";
                return res;
            }

            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                if (CheckDuplicateEmailID(ViewModel.UserID, ViewModel.EMailID, db))
                {
                    res.ExecutionResult = eExecutionResult.ValidationError;
                    res.ValidationError = "entered email id is already registered.";
                    return res;
                }
                else if (CheckDuplicate(ViewModel.UserID, ViewModel.BusinessName, db))
                {
                    res.ExecutionResult = eExecutionResult.ValidationError;
                    res.ValidationError = "Can not accept duplicate values. The business name is already exists.";
                    return res;
                }

                tblUser SaveModel = null;
                if (ViewModel.UserID == 0)
                {
                    SaveModel = new tblUser()
                    {
                        Password = ViewModel.Password ?? "",
                        UserRoleID = (int)Models.Users.eUserRoleID.Customer,
                        rcdt = DateTime.Now
                    };
                    if (Common.Props.LoginUser != null)
                    {
                        SaveModel.rcuid = Common.Props.LoginUser.UserID;
                    }
                    db.tblUsers.Add(SaveModel);
                }
                else
                {
                    SaveModel = db.tblUsers.FirstOrDefault(r => r.UserID == ViewModel.UserID);
                    if (SaveModel == null)
                    {
                        res.ExecutionResult = eExecutionResult.ValidationError;
                        res.ValidationError = "Selected user has been deleted over network. Can not find user's details. Please retry.";
                        return res;
                    }

                    SaveModel.redt = DateTime.Now;
                    SaveModel.reuid = Common.Props.LoginUser.UserID;

                    db.tblUsers.Attach(SaveModel);
                    db.Entry(SaveModel).State = System.Data.Entity.EntityState.Modified;
                }

                SaveModel.BusinessName = ViewModel.BusinessName;
                SaveModel.ContactName = ViewModel.ContactName ?? "";
                SaveModel.EmailID = ViewModel.EMailID;
                SaveModel.Address = ViewModel.Address ?? "";
                SaveModel.City = ViewModel.City ?? "";
                SaveModel.PostCode = ViewModel.Postcode ?? "";
                SaveModel.Country = ViewModel.Country ?? "";
                SaveModel.IntPhoneNo = ViewModel.IntPhoneNo ?? "";
                SaveModel.AirportDestCity = ViewModel.AirportDestCity ?? "";
                //--
                try
                {
                    db.SaveChanges();
                    res.PrimeKeyValue = SaveModel.UserID;
                    res.ExecutionResult = eExecutionResult.CommitedSucessfuly;
                }
                catch (Exception ex)
                {
                    ex = Common.Functions.FindFinalError(ex);

                    res.ExecutionResult = eExecutionResult.ErrorWhileExecuting;
                    res.Exception = ex;
                }
            }
            return res;
        }

        public BeforeDeleteValidationResult ValidateBeforeDelete(int ID)
        {
            BeforeDeleteValidationResult res = new BeforeDeleteValidationResult()
            {
                IsValidForDelete = true
            };
            return res;
            //using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            //{
            //    return ValidateBeforeDelete(ID, db);
            //}
        }
        public BeforeDeleteValidationResult ValidateBeforeDelete(int ID, dbUltraCoralEntities db)
        {
            BeforeDeleteValidationResult res = new BeforeDeleteValidationResult()
            {
                IsValidForDelete = true
            };

            //if (db.tblUsers.FirstOrDefault(r => r.SizeID == ID) != null)
            //{
            //    res.IsValidForDelete = false;
            //    res.ValidationMessage = "Already selected in Customers.";
            //}

            return res;
        }

        public SavingResult DeleteRecord(int ID)
        {
            SavingResult res = new SavingResult();

            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                var RecordToDelete = db.tblUsers.Find(ID);

                if (RecordToDelete == null)
                {
                    res.ExecutionResult = eExecutionResult.ValidationError;
                    res.ValidationError = "Selected record is already deleted or changed over network. Record not found.";
                    return res;
                }

                db.tblUsers.Remove(RecordToDelete);
                //--
                try
                {
                    db.SaveChanges();
                    res.ExecutionResult = eExecutionResult.CommitedSucessfuly;
                }
                catch (Exception ex)
                {
                    ex = Common.Functions.FindFinalError(ex);

                    res.ExecutionResult = eExecutionResult.ErrorWhileExecuting;
                    res.Exception = ex;
                }
            }
            return res;
        }

        public bool CheckDuplicate(int ID, string BusinessName)
        {
            using (DAL.dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                return CheckDuplicate(ID, BusinessName, db);
            }
        }
        public bool CheckDuplicate(int ID, string BusinessName, dbUltraCoralEntities db)
        {
            return db.tblUsers.FirstOrDefault(r => r.UserID != ID && r.BusinessName == BusinessName) != null;
        }

        public bool CheckDuplicateEmailID(int ID, string EMailID)
        {
            using (DAL.dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                return CheckDuplicateEmailID(ID, EMailID, db);
            }
        }
        public bool CheckDuplicateEmailID(int ID, string EMailID, dbUltraCoralEntities db)
        {
            return db.tblUsers.FirstOrDefault(r => r.UserID != ID && r.EmailID == EMailID) != null;
        }

        public SavingResult ApproveUser(int UserID, bool Approval)
        {
            SavingResult res = new Models.Template.SavingResult();
            using (dbUltraCoralEntities db = new DAL.dbUltraCoralEntities())
            {
                tblUser SaveModel = db.tblUsers.Find(UserID);
                if(SaveModel == null)
                {
                    res.ExecutionResult = eExecutionResult.ValidationError;
                    res.ValidationError = "Invalid request. User has been deleted or moved.";
                    return res;
                }

                SaveModel.IsApproved = Approval;
                SaveModel.redt = DateTime.Now;
                SaveModel.reuid = (Common.Props.LoginUser != null? (int?)Common.Props.LoginUser.UserID : null);
                db.tblUsers.Attach(SaveModel);
                db.Entry(SaveModel).State = System.Data.Entity.EntityState.Modified;

                //--
                try
                {
                    db.SaveChanges();
                    res.PrimeKeyValue = SaveModel.UserID;
                    res.ExecutionResult = eExecutionResult.CommitedSucessfuly;
                }
                catch (Exception ex)
                {
                    ex = Common.Functions.FindFinalError(ex);

                    res.ExecutionResult = eExecutionResult.ErrorWhileExecuting;
                    res.Exception = ex;
                }
            }
            return res;
        }

        public static SelectList GetSelectionList()
        {
            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                int CustomerRoleID = (int)Models.Users.eUserRoleID.Customer;

                return new SelectList((from r in db.tblUsers
                                       where r.UserRoleID == CustomerRoleID
                                       orderby r.ContactName, r.BusinessName
                                       select new
                                       {
                                           CustomerID = r.UserID,
                                           BusinessName = r.BusinessName,
                                       }).ToList(), "CustomerID", "BusinessName", null);
            }
        }
    }
}