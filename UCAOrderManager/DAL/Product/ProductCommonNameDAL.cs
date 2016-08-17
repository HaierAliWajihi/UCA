using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCAOrderManager.Models.Product;
using UCAOrderManager.Models.Template;

namespace UCAOrderManager.DAL.Product
{
    public class ProductCommonNameDAL
    {
        public List<ProductCommonNameViewModel> GetList()
        {
            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                return (from r in db.tblProductCommonNames
                        select new ProductCommonNameViewModel()
                        {
                            ProductCommonNameID = r.ProductCommonNameID,
                            ProductCommonName = r.ProductCommonName
                        }).ToList();
            }
        }

        public ProductCommonNameViewModel FindByID(int ID)
        {
            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                var r = db.tblProductCommonNames.Find(ID);
                if (r == null)
                {
                    return null;
                }
                else
                {
                    return new ProductCommonNameViewModel()
                    {
                        ProductCommonNameID = r.ProductCommonNameID,
                        ProductCommonName = r.ProductCommonName
                    };
                }
            }
        }

        public SavingResult SaveRecord(ProductCommonNameViewModel ViewModel)
        {
            SavingResult res = new SavingResult();

            if (String.IsNullOrWhiteSpace(ViewModel.ProductCommonName))
            {
                res.ExecutionResult = eExecutionResult.ValidationError;
                res.ValidationError = "Please enter common name";
                return res;
            }

            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                if (CheckDuplicate(ViewModel.ProductCommonNameID, ViewModel.ProductCommonName))
                {
                    res.ExecutionResult = eExecutionResult.ValidationError;
                    res.ValidationError = "Can not accept duplicate values. The common name is already exists.";
                    return res;
                }

                tblProductCommonName SaveModel = null;
                if (ViewModel.ProductCommonNameID == 0)
                {
                    SaveModel = new tblProductCommonName()
                    {
                        rcdt = DateTime.Now,
                        rcuid = Common.Props.LoginUser.UserID
                    };
                    db.tblProductCommonNames.Add(SaveModel);
                }
                else
                {
                    SaveModel = db.tblProductCommonNames.FirstOrDefault(r => r.ProductCommonNameID == ViewModel.ProductCommonNameID);
                    if (SaveModel == null)
                    {
                        res.ExecutionResult = eExecutionResult.ValidationError;
                        res.ValidationError = "Selected user has been deleted over network. Can not find user's details. Please retry.";
                        return res;
                    }

                    SaveModel.redt = DateTime.Now;
                    SaveModel.reuid = Common.Props.LoginUser.UserID;

                    db.tblProductCommonNames.Attach(SaveModel);
                    db.Entry(SaveModel).State = System.Data.Entity.EntityState.Modified;
                }

                SaveModel.ProductCommonName = ViewModel.ProductCommonName;

                //--
                try
                {
                    db.SaveChanges();
                    res.PrimeKeyValue = SaveModel.ProductCommonNameID;
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
            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                return ValidateBeforeDelete(ID, db);
            }
        }

        public BeforeDeleteValidationResult ValidateBeforeDelete(int ID, dbUltraCoralEntities db)
        {
            BeforeDeleteValidationResult res = new BeforeDeleteValidationResult()
            {
                IsValidForDelete = true
            };

            if (db.tblProducts.FirstOrDefault(r => r.CommonNameID == ID) != null)
            {
                res.IsValidForDelete = false;
                res.ValidationMessage = "Already selected in products.";
            }

            return res;
        }

        public SavingResult DeleteRecord(int ID)
        {
            SavingResult res = new SavingResult();

            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                BeforeDeleteValidationResult BeforeDeleteValres = ValidateBeforeDelete(ID, db);
                if (!BeforeDeleteValres.IsValidForDelete)
                {
                    res.ExecutionResult = eExecutionResult.ValidationError;
                    res.ValidationError = "Following errors occured while deleting. " + BeforeDeleteValres.ValidationMessage;
                    return res;
                }

                var RecordToDelete = db.tblProductCommonNames.Find(ID);

                if (RecordToDelete == null)
                {
                    res.ExecutionResult = eExecutionResult.ValidationError;
                    res.ValidationError = "Selected record is already deleted or changed over network. Record not found.";
                    return res;
                }

                db.tblProductCommonNames.Remove(RecordToDelete);
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

        public bool CheckDuplicate(int ID, string Value)
        {
            using (DAL.dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                return CheckDuplicate(ID, Value, db);
            }
        }
        public bool CheckDuplicate(int ID, string Value, dbUltraCoralEntities db)
        {
            return db.tblProductCommonNames.FirstOrDefault(r => r.ProductCommonNameID != ID && r.ProductCommonName == Value) != null;
        }

        public static SelectList GetSelectList()
        {
            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                var list = (from r in db.tblProductCommonNames
                        select new ProductCommonNameSelectListViewModel()
                        {
                            ProductCommonNameID = r.ProductCommonNameID,
                            ProductCommonName = r.ProductCommonName
                        }).ToList();
                return new SelectList(list, "ProductCommonNameID", "ProductCommonName");
            }
        }
    }
}