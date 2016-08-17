using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                return (from r in db.tblCustomers
                        select new CustomerListViewModel()
                        {
                            CustomerID = r.CustomerID,
                            BusinessName = r.BusinessName,
                            ContactName = r.ContactName,
                            EMailID = r.EMailID,
                            Address = r.Address,
                            City = r.City,
                            Country = r.Country
                        }).ToList();
            }
        }

        public CustomerViewModel FindByID(int ID)
        {
            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                var r = db.tblCustomers.Find(ID);
                if (r == null)
                {
                    return null;
                }

                return new CustomerViewModel()
                {
                    CustomerID = r.CustomerID,
                    BusinessName = r.BusinessName,
                    ContactName = r.ContactName,
                    EMailID = r.EMailID,
                    Address = r.Address,
                    City = r.City,
                    Country = r.Country,
                    IntPhoneNo = r.IntPhoneNo,
                    AirportDestCity = r.AirportDestCity
                };
            }
        }

        public CustomerViewModel FindByIDGetListViewModel(int ID)
        {
            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                return (from r in db.tblCustomers
                        where r.CustomerID == ID

                        select new CustomerViewModel()
                        {
                            CustomerID = r.CustomerID,
                            BusinessName = r.BusinessName,
                            ContactName = r.ContactName,
                            EMailID = r.EMailID,
                            Address = r.Address,
                            City = r.City,
                            Country = r.Country
                        }).FirstOrDefault();
            }
        }

        public SavingResult SaveRecord(CustomerViewModel ViewModel)
        {
            SavingResult res = new SavingResult();

            if (ViewModel.BusinessName == "")
            {
                res.ExecutionResult = eExecutionResult.ValidationError;
                res.ValidationError = "Business Name is required";
                return res;
            }

            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                if (CheckDuplicate(ViewModel.CustomerID, ViewModel.BusinessName))
                {
                    res.ExecutionResult = eExecutionResult.ValidationError;
                    res.ValidationError = "Can not accept duplicate values. The business name is already exists.";
                    return res;
                }

                tblCustomer SaveModel = null;
                if (ViewModel.CustomerID == 0)
                {
                    SaveModel = new tblCustomer()
                    {
                        rcdt = DateTime.Now,
                        rcuid = Common.Props.LoginUser.UserID
                    };
                    db.tblCustomers.Add(SaveModel);
                }
                else
                {
                    SaveModel = db.tblCustomers.FirstOrDefault(r => r.CustomerID == ViewModel.CustomerID);
                    if (SaveModel == null)
                    {
                        res.ExecutionResult = eExecutionResult.ValidationError;
                        res.ValidationError = "Selected user has been deleted over network. Can not find user's details. Please retry.";
                        return res;
                    }

                    SaveModel.redt = DateTime.Now;
                    SaveModel.reuid = Common.Props.LoginUser.UserID;

                    db.tblCustomers.Attach(SaveModel);
                    db.Entry(SaveModel).State = System.Data.Entity.EntityState.Modified;
                }

                SaveModel.BusinessName = ViewModel.BusinessName;
                SaveModel.ContactName = ViewModel.ContactName;
                SaveModel.EMailID = ViewModel.EMailID;
                SaveModel.Address = ViewModel.Address;
                SaveModel.City = ViewModel.City;
                SaveModel.Country = ViewModel.Country;
                SaveModel.IntPhoneNo = ViewModel.IntPhoneNo;
                SaveModel.AirportDestCity = ViewModel.AirportDestCity;
                //--
                try
                {
                    db.SaveChanges();
                    res.PrimeKeyValue = SaveModel.CustomerID;
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

            //if (db.tblCustomers.FirstOrDefault(r => r.SizeID == ID) != null)
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
                var RecordToDelete = db.tblCustomers.Find(ID);

                if (RecordToDelete == null)
                {
                    res.ExecutionResult = eExecutionResult.ValidationError;
                    res.ValidationError = "Selected record is already deleted or changed over network. Record not found.";
                    return res;
                }

                db.tblCustomers.Remove(RecordToDelete);
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
            return db.tblCustomers.FirstOrDefault(r => r.CustomerID != ID && r.BusinessName == BusinessName) != null;
        }

    }
}