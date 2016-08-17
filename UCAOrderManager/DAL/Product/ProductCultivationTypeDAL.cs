using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCAOrderManager.Models.Product;
using UCAOrderManager.Models.Template;

namespace UCAOrderManager.DAL.Product
{
    public class ProductCultivationTypeDAL
    {
        public List<ProductCultivationTypeViewModel> GetList()
        {
            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                return (from r in db.tblProductCultivationTypes
                        select new ProductCultivationTypeViewModel()
                        {
                            ProductCultivationTypeID = r.ProductCultivationTypeID,
                            ProductCultivationTypeName = r.ProductCultivationTypeName
                        }).ToList();
            }
        }

        public ProductCultivationTypeViewModel FindByID(int ID)
        {
            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                var r = db.tblProductCultivationTypes.Find(ID);
                if (r == null)
                {
                    return null;
                }
                else
                {
                    return new ProductCultivationTypeViewModel()
                    {
                        ProductCultivationTypeID = r.ProductCultivationTypeID,
                        ProductCultivationTypeName = r.ProductCultivationTypeName
                    };
                }
            }
        }

        public SavingResult SaveRecord(ProductCultivationTypeViewModel ViewModel)
        {
            SavingResult res = new SavingResult();

            if (String.IsNullOrWhiteSpace(ViewModel.ProductCultivationTypeName))
            {
                res.ExecutionResult = eExecutionResult.ValidationError;
                res.ValidationError = "Please enter Cultivation Type name";
                return res;
            }

            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                if (CheckDuplicate(ViewModel.ProductCultivationTypeID, ViewModel.ProductCultivationTypeName))
                {
                    res.ExecutionResult = eExecutionResult.ValidationError;
                    res.ValidationError = "Can not accept duplicate values. The Cultivation Type name is already exists.";
                    return res;
                }

                tblProductCultivationType SaveModel = null;
                if (ViewModel.ProductCultivationTypeID == 0)
                {
                    SaveModel = new tblProductCultivationType()
                    {
                        rcdt = DateTime.Now,
                        rcuid = Common.Props.LoginUser.UserID
                    };
                    db.tblProductCultivationTypes.Add(SaveModel);
                }
                else
                {
                    SaveModel = db.tblProductCultivationTypes.FirstOrDefault(r => r.ProductCultivationTypeID == ViewModel.ProductCultivationTypeID);
                    if (SaveModel == null)
                    {
                        res.ExecutionResult = eExecutionResult.ValidationError;
                        res.ValidationError = "Selected user has been deleted over network. Can not find user's details. Please retry.";
                        return res;
                    }

                    SaveModel.redt = DateTime.Now;
                    SaveModel.reuid = Common.Props.LoginUser.UserID;

                    db.tblProductCultivationTypes.Attach(SaveModel);
                    db.Entry(SaveModel).State = System.Data.Entity.EntityState.Modified;
                }

                SaveModel.ProductCultivationTypeName = ViewModel.ProductCultivationTypeName;

                //--
                try
                {
                    db.SaveChanges();
                    res.PrimeKeyValue = SaveModel.ProductCultivationTypeID;
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

            if (db.tblProducts.FirstOrDefault(r => r.CultivationTypeID == ID) != null)
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

                var RecordToDelete = db.tblProductCultivationTypes.Find(ID);

                if (RecordToDelete == null)
                {
                    res.ExecutionResult = eExecutionResult.ValidationError;
                    res.ValidationError = "Selected record is already deleted or changed over network. Record not found.";
                    return res;
                }

                db.tblProductCultivationTypes.Remove(RecordToDelete);
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
            return db.tblProductCultivationTypes.FirstOrDefault(r => r.ProductCultivationTypeID != ID && r.ProductCultivationTypeName == Value) != null;
        }

        public static SelectList GetSelectList()
        {
            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                var list = (from r in db.tblProductCultivationTypes
                        select new ProductCultivationTypeSelectListViewModel()
                        {
                            ProductCultivationTypeID = r.ProductCultivationTypeID,
                            ProductCultivationTypeName = r.ProductCultivationTypeName
                        }).ToList();

                return new SelectList(list, "ProductCultivationTypeID", "ProductCultivationTypeName");
            }
        }
    }
}