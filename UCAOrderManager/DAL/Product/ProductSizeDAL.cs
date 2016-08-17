using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCAOrderManager.Models.Product;
using UCAOrderManager.Models.Template;

namespace UCAOrderManager.DAL.Product
{
    public class ProductSizeDAL
    {
        public List<ProductSizeViewModel> GetList()
        {
            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                return (from r in db.tblProductSizes
                        select new ProductSizeViewModel()
                        {
                            ProductSizeID = r.ProductSizeID,
                            ProductSizeName = r.ProductSizeName
                        }).ToList();
            }
        }

        public ProductSizeViewModel FindByID(int ID)
        {
            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                var r = db.tblProductSizes.Find(ID);
                if (r == null)
                {
                    return null;
                }
                else
                {
                    return new ProductSizeViewModel()
                    {
                        ProductSizeID = r.ProductSizeID,
                        ProductSizeName = r.ProductSizeName
                    };
                }
            }
        }

        public SavingResult SaveRecord(ProductSizeViewModel ViewModel)
        {
            SavingResult res = new SavingResult();

            if (String.IsNullOrWhiteSpace(ViewModel.ProductSizeName))
            {
                res.ExecutionResult = eExecutionResult.ValidationError;
                res.ValidationError = "Please enter size name";
                return res;
            }

            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                if (CheckDuplicate(ViewModel.ProductSizeID, ViewModel.ProductSizeName))
                {
                    res.ExecutionResult = eExecutionResult.ValidationError;
                    res.ValidationError = "Can not accept duplicate values. The size name is already exists.";
                    return res;
                }

                tblProductSize SaveModel = null;
                if (ViewModel.ProductSizeID == 0)
                {
                    SaveModel = new tblProductSize()
                    {
                        rcdt = DateTime.Now,
                        rcuid = Common.Props.LoginUser.UserID
                    };
                    db.tblProductSizes.Add(SaveModel);
                }
                else
                {
                    SaveModel = db.tblProductSizes.FirstOrDefault(r => r.ProductSizeID == ViewModel.ProductSizeID);
                    if (SaveModel == null)
                    {
                        res.ExecutionResult = eExecutionResult.ValidationError;
                        res.ValidationError = "Selected user has been deleted over network. Can not find user's details. Please retry.";
                        return res;
                    }

                    SaveModel.redt = DateTime.Now;
                    SaveModel.reuid = Common.Props.LoginUser.UserID;

                    db.tblProductSizes.Attach(SaveModel);
                    db.Entry(SaveModel).State = System.Data.Entity.EntityState.Modified;
                }

                SaveModel.ProductSizeName = ViewModel.ProductSizeName;

                //--
                try
                {
                    db.SaveChanges();
                    res.PrimeKeyValue = SaveModel.ProductSizeID;
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
            using(dbUltraCoralEntities db = new dbUltraCoralEntities())
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

            if (db.tblProducts.FirstOrDefault(r => r.SizeID == ID) != null)
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
                if(!BeforeDeleteValres.IsValidForDelete)
                {
                    res.ExecutionResult = eExecutionResult.ValidationError;
                    res.ValidationError = "Following errors occured while deleting. " + BeforeDeleteValres.ValidationMessage;
                    return res;
                }

                var RecordToDelete = db.tblProductSizes.Find(ID);

                if (RecordToDelete == null)
                {
                    res.ExecutionResult = eExecutionResult.ValidationError;
                    res.ValidationError = "Selected record is already deleted or changed over network. Record not found.";
                    return res;
                }

                db.tblProductSizes.Remove(RecordToDelete);
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
            return db.tblProductSizes.FirstOrDefault(r => r.ProductSizeID != ID && r.ProductSizeName == Value) != null;
        }

        public static SelectList GetSelectList()
        {
            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                var list = (from r in db.tblProductSizes
                        select new ProductSizeSelectListViewModel()
                        {
                            ProductSizeID = r.ProductSizeID,
                            ProductSizeName = r.ProductSizeName
                        }).ToList();

                return new SelectList(list, "ProductSizeID", "ProductSizeName");
            }
        }
    }
}