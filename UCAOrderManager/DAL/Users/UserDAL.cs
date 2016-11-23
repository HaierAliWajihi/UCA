using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UCAOrderManager.Models.Template;
using UCAOrderManager.Models.Users;

namespace UCAOrderManager.DAL.Users
{
    public class UserDAL
    {
        public SavingResult SaveRecord(UserRegistrationViewModel ViewModel)
        {
            SavingResult res = new SavingResult();

            if(String.IsNullOrWhiteSpace(ViewModel.FullName))
            {
                res.ExecutionResult = eExecutionResult.ValidationError;
                res.ValidationError = "Please enter full name";
                return res;
            }
            else if(String.IsNullOrWhiteSpace(ViewModel.EMailID))
            {
                res.ExecutionResult = eExecutionResult.ValidationError;
                res.ValidationError = "Please enter email id";
                return res;
            }
            else if (String.IsNullOrWhiteSpace(ViewModel.EMailID))
            {
                res.ExecutionResult = eExecutionResult.ValidationError;
                res.ValidationError = "Please enter password";
                return res;
            }

            using(dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                if (CheckDuplicate(ViewModel.UserID, ViewModel.EMailID, db))
                {
                    res.ExecutionResult = eExecutionResult.ValidationError;
                    res.ValidationError = "Email already exists";
                    return res;
                }

                tblUser SaveModel = null;
                if (ViewModel.UserID == 0)
                {
                    SaveModel = new tblUser()
                    {
                        UserRoleID = (int)eUserRoleID.Admin,
                        IsApproved = true,
                        rcdt = DateTime.Now
                    };
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

                    db.tblUsers.Attach(SaveModel);
                    db.Entry(SaveModel).State = System.Data.Entity.EntityState.Modified;
                }
                
                SaveModel.FullName = ViewModel.FullName;
                SaveModel.EmailID = ViewModel.EMailID;
                SaveModel.Password = ViewModel.Password;

                //--
                try
                {
                    db.SaveChanges();
                    res.PrimeKeyValue = SaveModel.UserID;
                    res.ExecutionResult = eExecutionResult.CommitedSucessfuly;
                }
                catch (Exception ex)
                {
                    while (ex.Message == "An error occurred while updating the entries. See the inner exception for details.")
                    {
                        ex = ex.InnerException;
                    }

                    res.ExecutionResult = eExecutionResult.ErrorWhileExecuting;
                    res.Exception = ex;
                }
            }

            return res;
        }

        public UserLoginDetails MatchUserCredentials(string EMailID, string password)
        {
            using(dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                tblUser User = db.tblUsers.FirstOrDefault(r => r.EmailID == EMailID && r.Password == password);
                if(User == null)
                {
                    return null;
                }
                return new UserLoginDetails()
                {
                    UserID = User.UserID,
                    EMailID = User.EmailID,
                    FullName = User.FullName ?? User.ContactName ?? User.BusinessName ?? "NoName",
                    IsApproved = User.IsApproved,
                    Role = (eUserRoleID)User.UserRoleID
                };
            }
        }

        public SavingResult ChangePassword(int UserID, string NewPassword)
        {
            SavingResult res = new SavingResult();

            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                tblUser SaveModel = null;
                SaveModel = db.tblUsers.FirstOrDefault(r => r.UserID == UserID);
                if (SaveModel == null)
                {
                    res.ExecutionResult = eExecutionResult.ValidationError;
                    res.ValidationError = "Selected user has been deleted over network. Can not find user's details. Please retry.";
                    return res;
                }

                SaveModel.Password = NewPassword;
                SaveModel.redt = DateTime.Now;

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
                    while (ex.Message == "An error occurred while updating the entries. See the inner exception for details.")
                    {
                        ex = ex.InnerException;
                    }

                    res.ExecutionResult = eExecutionResult.ErrorWhileExecuting;
                    res.Exception = ex;
                }
            }

            return res;
        }

        public List<Models.Users.UserApprovalViewModel> GetPendingApprovalUserList()
        {
            using(dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                return db.tblUsers.Where(r => !(r.IsApproved ?? false)).Select<tblUser, Models.Users.UserApprovalViewModel>(r =>
                    new Models.Users.UserApprovalViewModel()
                    {
                        UserID = r.UserID,
                        FullName = r.FullName,
                        EMailID = r.EmailID
                    }).ToList();
            }
        }

        public SavingResult ApproveUser(int UserID)
        {
            SavingResult res = new SavingResult();

            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                tblUser SaveModel = null;
                SaveModel = db.tblUsers.FirstOrDefault(r => r.UserID == UserID);
                if (SaveModel == null)
                {
                    res.ExecutionResult = eExecutionResult.ValidationError;
                    res.ValidationError = "Selected user has been deleted over network. Can not find user's details. Please retry.";
                    return res;
                }

                SaveModel.IsApproved = true;
                SaveModel.redt = DateTime.Now;

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
                    while (ex.Message == "An error occurred while updating the entries. See the inner exception for details.")
                    {
                        ex = ex.InnerException;
                    }

                    res.ExecutionResult = eExecutionResult.ErrorWhileExecuting;
                    res.Exception = ex;
                }
            }

            return res;
        }

        public bool CheckDuplicate(int ID, string Value)
        {
            using (DAL.dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                return CheckDuplicate(ID, Value, db);
            }
        }
        public bool CheckDuplicate(int ID, string Value, dbUltraCoralEntities db)
        {
            return db.tblUsers.FirstOrDefault(r => r.UserID != ID && r.EmailID == Value) != null;
        }

        public List<Models.Users.UserAdminListViewModel> GetAdminUsers()
        {
            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                int AdminRoleID = (int)eUserRoleID.Admin;
                return db.tblUsers.Where(r => r.UserRoleID == AdminRoleID && (r.IsApproved ?? false)).Select<tblUser, Models.Users.UserAdminListViewModel>(r =>
                    new Models.Users.UserAdminListViewModel()
                    {
                        UserID = r.UserID,
                        FullName = r.FullName,
                        EMailID = r.EmailID
                    }).ToList();
            }
        }
    }
}