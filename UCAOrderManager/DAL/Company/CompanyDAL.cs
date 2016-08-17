using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using UCAOrderManager.Models.Company;
using UCAOrderManager.Models.Template;

namespace UCAOrderManager.DAL.Company
{
    public class CompanyDAL
    {
        public SavingResult SaveRecord(CompanyViewModel ViewModel)
        {
            SavingResult res = new SavingResult();

            if (String.IsNullOrWhiteSpace(ViewModel.CompanyName))
            {
                res.ExecutionResult = eExecutionResult.ValidationError;
                res.ValidationError = "Please enter company name";
                return res;
            }

            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                tblCompany SaveModel = null;
                if (ViewModel.CompanyID == 0)
                {
                    SaveModel = new tblCompany()
                    {
                        rcdt = DateTime.Now
                    };
                    db.tblCompanies.Add(SaveModel);
                }
                else
                {
                    SaveModel = db.tblCompanies.FirstOrDefault(r => r.CompanyID == ViewModel.CompanyID);
                    if (SaveModel == null)
                    {
                        res.ExecutionResult = eExecutionResult.ValidationError;
                        res.ValidationError = "Selected user has been deleted over network. Can not find user's details. Please retry.";
                        return res;
                    }

                    SaveModel.redt = DateTime.Now;

                    db.tblCompanies.Attach(SaveModel);
                    db.Entry(SaveModel).State = System.Data.Entity.EntityState.Modified;
                }

                SaveModel.CompanyName = ViewModel.CompanyName;
                SaveModel.Address = ViewModel.Address;
                SaveModel.PhoneNo = ViewModel.PhoneNo;
                SaveModel.FaxNo = ViewModel.FaxNo;
                SaveModel.EMailID = ViewModel.EMailID;
                SaveModel.Website = ViewModel.Website;
                SaveModel.Slogan = ViewModel.Slogan;
                //SaveModel.Logo = ViewModel.Logo;
                //--
                try
                {
                    db.SaveChanges();
                    res.PrimeKeyValue = SaveModel.CompanyID;
                    res.ExecutionResult = eExecutionResult.CommitedSucessfuly;
                }
                catch (Exception ex)
                {
                    if (ex.GetType() == typeof(DbEntityValidationException))
                    {
                        string err = "";
                        DbEntityValidationException dbVErr = (DbEntityValidationException)ex;
                        foreach (var x in dbVErr.EntityValidationErrors)
                        {
                            foreach (var e in x.ValidationErrors)
                            {
                                err = err + "\r\n" + e.PropertyName + " = " + e.ErrorMessage + ".";
                            }
                        }
                        ex = new Exception(err);
                    }
                    else
                    {
                        while (ex.Message == "An error occurred while updating the entries. See the inner exception for details.")
                        {
                            ex = ex.InnerException;
                        }
                    }

                    res.ExecutionResult = eExecutionResult.ErrorWhileExecuting;
                    res.Exception = ex;
                }
            }
            return res;
        }

        public CompanyViewModel GetFirstCompanyDetail()
        {
            using(dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                tblCompany SaveModel = db.tblCompanies.FirstOrDefault();
                if (SaveModel == null) return null;
                return new CompanyViewModel()
                {
                    CompanyID = SaveModel.CompanyID,
                    CompanyName = SaveModel.CompanyName,
                    Address = SaveModel.Address, 
                    PhoneNo = SaveModel.PhoneNo, 
                    FaxNo = SaveModel.FaxNo,
                    EMailID = SaveModel.EMailID, 
                    Website = SaveModel.Website,
                    Slogan = SaveModel.Slogan
                };
            }
        }
    }
}