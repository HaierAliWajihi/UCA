using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCAOrderManager.Models.Product;
using UCAOrderManager.Models.Template;

namespace UCAOrderManager.DAL.Product
{
    public class ProductScientificNameDAL
    {
        public List<ProductScientificNameViewModel> GetList()
        {
            using(dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                return (from r in db.tblProductScientificNames
                        orderby r.ProductScientificName
                        select new ProductScientificNameViewModel()
                        {
                            ProductScientificNameID = r.ProductScientificNameID,
                            ProductScientificName = r.ProductScientificName,
                            IsAlive = r.IsAlive
                        }).ToList();
            }
        }

        public ProductScientificNameViewModel FindByID(int ID)
        {
            using(dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                var r = db.tblProductScientificNames.Find(ID);
                if (r == null)
                {
                    return null;
                }
                else
                {
                    return new ProductScientificNameViewModel()
                        {
                            ProductScientificNameID = r.ProductScientificNameID,
                            ProductScientificName = r.ProductScientificName,
                            IsAlive = r.IsAlive
                        };
                }
            }
        }

        public SavingResult SaveRecord(ProductScientificNameViewModel ViewModel)
        {
            SavingResult res = new SavingResult();

            if (String.IsNullOrWhiteSpace(ViewModel.ProductScientificName))
            {
                res.ExecutionResult = eExecutionResult.ValidationError;
                res.ValidationError = "Please enter scientific name";
                return res;
            }

            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                if(CheckDuplicate(ViewModel.ProductScientificNameID, ViewModel.ProductScientificName))
                {
                    res.ExecutionResult = eExecutionResult.ValidationError;
                    res.ValidationError = "Can not accept duplicate values. Scientific name is already exists.";
                    return res;
                }

                tblProductScientificName SaveModel = null;
                if (ViewModel.ProductScientificNameID == 0)
                {
                    SaveModel = new tblProductScientificName()
                    {
                        rcdt = DateTime.Now,
                        rcuid = Common.Props.LoginUser.UserID
                    };
                    db.tblProductScientificNames.Add(SaveModel);
                }
                else
                {
                    SaveModel = db.tblProductScientificNames.FirstOrDefault(r => r.ProductScientificNameID == ViewModel.ProductScientificNameID);
                    if (SaveModel == null)
                    {
                        res.ExecutionResult = eExecutionResult.ValidationError;
                        res.ValidationError = "Selected user has been deleted over network. Can not find user's details. Please retry.";
                        return res;
                    }

                    SaveModel.redt = DateTime.Now;
                    SaveModel.reuid = Common.Props.LoginUser.UserID;

                    db.tblProductScientificNames.Attach(SaveModel);
                    db.Entry(SaveModel).State = System.Data.Entity.EntityState.Modified;
                }

                SaveModel.ProductScientificName = ViewModel.ProductScientificName;
                SaveModel.IsAlive = ViewModel.IsAlive;

                //--
                try
                {
                    db.SaveChanges();
                    res.PrimeKeyValue = SaveModel.ProductScientificNameID;
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

            if (db.tblProducts.FirstOrDefault(r => r.ScientificNameID == ID) != null)
            {
                res.IsValidForDelete = false;
                res.ValidationMessage = "Already selected in products.";
            }

            return res;
        }

        public SavingResult DeleteRecord(int ID)
        {
            SavingResult res = new SavingResult();

            using(dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                BeforeDeleteValidationResult BeforeDeleteValres = ValidateBeforeDelete(ID, db);
                if (!BeforeDeleteValres.IsValidForDelete)
                {
                    res.ExecutionResult = eExecutionResult.ValidationError;
                    res.ValidationError = "Following errors occured while deleting. " + BeforeDeleteValres.ValidationMessage;
                    return res;
                }

                var RecordToDelete = db.tblProductScientificNames.Find(ID);
                
                if(RecordToDelete == null)
                {
                    res.ExecutionResult = eExecutionResult.ValidationError;
                    res.ValidationError = "Selected record is already deleted or changed over network. Record not found.";
                    return res;
                }

                db.tblProductScientificNames.Remove(RecordToDelete);
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
            using(DAL.dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                return CheckDuplicate(ID, Value, db);
            }
        }
        public bool CheckDuplicate(int ID, string Value, dbUltraCoralEntities db)
        {
            return db.tblProductScientificNames.FirstOrDefault(r => r.ProductScientificNameID != ID && r.ProductScientificName == Value) != null;
        }

        public static SelectList GetSelectList()
        {
            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                var list = (from r in db.tblProductScientificNames
                        where r.IsAlive
                        orderby r.ProductScientificName
                        select new ProductScientificNameSelectListViewModel()
                        {
                            ProductScientificNameID = r.ProductScientificNameID,
                            ProductScientificName = r.ProductScientificName
                        }).ToList();
                return new SelectList(list, "ProductScientificNameID", "ProductScientificName");
            }
        }

    }
}