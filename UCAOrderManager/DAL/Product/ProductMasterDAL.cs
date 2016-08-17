using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UCAOrderManager.Models.Product;
using UCAOrderManager.Models.Template;

namespace UCAOrderManager.DAL.Product
{
    public class ProductMasterDAL
    {
        public List<ProductMasterListViewModel> GetList()
        {
            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                return (from r in db.tblProducts
                        join sn in db.tblProductScientificNames on r.ScientificNameID equals sn.ProductScientificNameID into joinsn
                        from rsn in joinsn.DefaultIfEmpty()
                        join cn in db.tblProductCommonNames on r.CommonNameID equals cn.ProductCommonNameID into joincn
                        from rcn in joincn.DefaultIfEmpty()
                        join size in db.tblProductSizes on r.SizeID equals size.ProductSizeID into joinsize
                        from rsize in joinsize.DefaultIfEmpty()
                        join ct in db.tblProductCultivationTypes on r.CultivationTypeID equals ct.ProductCultivationTypeID into joinct
                        from rct in joinct.DefaultIfEmpty()

                        select new ProductMasterListViewModel()
                        {
                            ProductID = r.ProductID,
                            ProductCode = r.ProductCode,
                            ScientificName = (rct != null ? rsn.ProductScientificName : ""),
                            CommonName = (rcn != null ? rcn.ProductCommonName : ""),
                            Descr = r.Descr,
                            SizeName = (rsize != null ? rsize.ProductSizeName : ""),
                            CultivationTypeName = (rct != null ? rct.ProductCultivationTypeName : ""),
                            Rate = r.Rate,
                            RateUplift = r.RateUpliftPerc,
                            CurrentStock = r.CurrentStock
                        }).ToList();
            }
        }

        public ProductMasterViewModel FindByID(int ID)
        {
            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                var r = db.tblProducts.Find(ID);
                if(r == null)
                {
                    return null;
                }

                return new ProductMasterViewModel()
                {
                    ProductID = r.ProductID,
                    ProductCode = r.ProductCode,
                    ScientificNameID = r.ScientificNameID,
                    CommonNameID = r.CommonNameID,
                    Descr = r.Descr,
                    SizeID = r.SizeID,
                    CultivationTypeID = r.CultivationTypeID,
                    Rate = r.Rate,
                    RateUplift = r.RateUpliftPerc,
                    CurrentStock = r.CurrentStock
                };
            }
        }
        public ProductMasterListViewModel FindByIDGetListViewModel(int ID)
        {
            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                return (from r in db.tblProducts
                        join sn in db.tblProductScientificNames on r.ScientificNameID equals sn.ProductScientificNameID into joinsn
                        from rsn in joinsn.DefaultIfEmpty()
                        join cn in db.tblProductCommonNames on r.CommonNameID equals cn.ProductCommonNameID into joincn
                        from rcn in joincn.DefaultIfEmpty()
                        join size in db.tblProductSizes on r.SizeID equals size.ProductSizeID into joinsize
                        from rsize in joinsize.DefaultIfEmpty()
                        join ct in db.tblProductCultivationTypes on r.CultivationTypeID equals ct.ProductCultivationTypeID into joinct
                        from rct in joinct.DefaultIfEmpty()
                        
                        where r.ProductID == ID

                        select new ProductMasterListViewModel()
                        {
                            ProductID = r.ProductID,
                            ProductCode = r.ProductCode,
                            ScientificName = (rct != null ? rsn.ProductScientificName : ""),
                            CommonName = (rcn != null ? rcn.ProductCommonName : ""),
                            Descr = r.Descr,
                            SizeName = (rsize != null ? rsize.ProductSizeName : ""),
                            CultivationTypeName = (rct != null ? rct.ProductCultivationTypeName : ""),
                            Rate = r.Rate,
                            RateUplift = r.RateUpliftPerc,
                            CurrentStock = r.CurrentStock
                        }).FirstOrDefault();
            }
        }

        public SavingResult SaveRecord(ProductMasterViewModel ViewModel)
        {
            SavingResult res = new SavingResult();

            if (ViewModel.ProductCode == 0)
            {
                res.ExecutionResult = eExecutionResult.ValidationError;
                res.ValidationError = "Product code is required";
                return res;
            }

            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                if (CheckDuplicate(ViewModel.ProductID, ViewModel.ScientificNameID, ViewModel.CommonNameID, ViewModel.Descr, ViewModel.SizeID, ViewModel.CultivationTypeID))
                {
                    res.ExecutionResult = eExecutionResult.ValidationError;
                    res.ValidationError = "Can not accept duplicate values. The product is already exists.";
                    return res;
                }

                tblProduct SaveModel = null;
                if (ViewModel.ProductID == 0)
                {
                    SaveModel = new tblProduct()
                    {
                        rcdt = DateTime.Now,
                        rcuid = Common.Props.LoginUser.UserID
                    };
                    db.tblProducts.Add(SaveModel);
                }
                else
                {
                    SaveModel = db.tblProducts.FirstOrDefault(r => r.ProductID == ViewModel.ProductID);
                    if (SaveModel == null)
                    {
                        res.ExecutionResult = eExecutionResult.ValidationError;
                        res.ValidationError = "Selected user has been deleted over network. Can not find user's details. Please retry.";
                        return res;
                    }

                    SaveModel.redt = DateTime.Now;
                    SaveModel.reuid = Common.Props.LoginUser.UserID;

                    db.tblProducts.Attach(SaveModel);
                    db.Entry(SaveModel).State = System.Data.Entity.EntityState.Modified;
                }

                SaveModel.ProductCode = ViewModel.ProductCode;
                SaveModel.ScientificNameID = ViewModel.ScientificNameID;
                SaveModel.CommonNameID = ViewModel.CommonNameID;
                SaveModel.Descr = ViewModel.Descr;
                SaveModel.SizeID = ViewModel.SizeID;
                SaveModel.CultivationTypeID = ViewModel.CultivationTypeID;
                
                SaveModel.Rate = ViewModel.Rate;
                SaveModel.RateUpliftPerc = ViewModel.RateUplift;
                SaveModel.UpliftedRate = ViewModel.Rate + Math.Round((ViewModel.Rate * ViewModel.RateUplift) / 100, 2);
                
                SaveModel.CurrentStock = ViewModel.CurrentStock;

                //--
                try
                {
                    db.SaveChanges();
                    res.PrimeKeyValue = SaveModel.ProductID;
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

            //if (db.tblProducts.FirstOrDefault(r => r.SizeID == ID) != null)
            //{
            //    res.IsValidForDelete = false;
            //    res.ValidationMessage = "Already selected in products.";
            //}

            return res;
        }

        public SavingResult DeleteRecord(int ID)
        {
            SavingResult res = new SavingResult();

            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                var RecordToDelete = db.tblProducts.Find(ID);

                if (RecordToDelete == null)
                {
                    res.ExecutionResult = eExecutionResult.ValidationError;
                    res.ValidationError = "Selected record is already deleted or changed over network. Record not found.";
                    return res;
                }

                db.tblProducts.Remove(RecordToDelete);
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

        public bool CheckDuplicate(int ID, int ScientificNameID, int CommonNameID, string Descr, int SizeID, int CultivationTypeID)
        {
            using (DAL.dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                return CheckDuplicate(ID, ScientificNameID, CommonNameID, Descr, SizeID, CultivationTypeID, db);
            }
        }
        public bool CheckDuplicate(int ID, int ScientificNameID, int CommonNameID, string Descr, int SizeID, int CultivationTypeID,
            dbUltraCoralEntities db)
        {
            return db.tblProducts.FirstOrDefault(r => r.ProductID != ID && 
                r.ScientificNameID == ScientificNameID &&
                r.CommonNameID == CommonNameID &&
                r.Descr == Descr &&
                r.SizeID == SizeID &&
                r.CultivationTypeID == CultivationTypeID) != null;
        }

        public int GenerateNewProductCode()
        {
            using(dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                return (db.tblProducts.Max(r => (int?)r.ProductCode) ?? 0) + 1;
            }
        }

        public SavingResult UpdateCurrentStock(int ProductID, decimal Quan)
        {
            SavingResult res = new SavingResult();

            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                tblProduct SaveModel = db.tblProducts.Find(ProductID);
                if (SaveModel == null)
                {
                    res.ExecutionResult = eExecutionResult.ValidationError;
                    res.ValidationError = "Product not found";
                    return res;
                }

                SaveModel = db.tblProducts.FirstOrDefault(r => r.ProductID == ProductID);
                if (SaveModel == null)
                {
                    res.ExecutionResult = eExecutionResult.ValidationError;
                    res.ValidationError = "Selected user has been deleted over network. Can not find product details. Please retry.";
                    return res;
                }

                if (SaveModel.CurrentStock == Quan) // if quantity has not changed then don't update quantity.
                {
                    return new SavingResult()
                    {
                        ExecutionResult = eExecutionResult.NotExecutedYet
                    };
                }

                SaveModel.CurrentStock = Quan;
                SaveModel.redt = DateTime.Now;
                //SaveModel.reuid = Common.Props.LoginUser.UserID;

                db.tblProducts.Attach(SaveModel);
                db.Entry(SaveModel).State = System.Data.Entity.EntityState.Modified;

                //--
                db.tblUpdateInventoryLogs.Add(new tblUpdateInventoryLog()
                {
                    ProductID = ProductID,
                    Quan = Quan,
                    UserID = Common.Props.LoginUser.UserID,
                    LogDT = DateTime.Now
                });
                //--
                try
                {
                    db.SaveChanges();
                    res.PrimeKeyValue = SaveModel.ProductID;
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

        public SavingResult UpdateRate(int ProductID, decimal Rate)
        {
            SavingResult res = new SavingResult();

            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                tblProduct SaveModel = db.tblProducts.Find(ProductID);
                if (SaveModel == null)
                {
                    res.ExecutionResult = eExecutionResult.ValidationError;
                    res.ValidationError = "Product not found";
                    return res;
                }

                SaveModel = db.tblProducts.FirstOrDefault(r => r.ProductID == ProductID);
                if (SaveModel == null)
                {
                    res.ExecutionResult = eExecutionResult.ValidationError;
                    res.ValidationError = "Selected user has been deleted over network. Can not find product details. Please retry.";
                    return res;
                }

                if (SaveModel.Rate == Rate) // if quantity has not changed then don't update quantity.
                {
                    return new SavingResult()
                    {
                        ExecutionResult = eExecutionResult.NotExecutedYet
                    };
                }

                SaveModel.Rate = Rate;
                SaveModel.redt = DateTime.Now;
                //SaveModel.reuid = Common.Props.LoginUser.UserID;

                db.tblProducts.Attach(SaveModel);
                db.Entry(SaveModel).State = System.Data.Entity.EntityState.Modified;

                ////--
                //db.tblUpdateInventoryLogs.Add(new tblUpdateInventoryLog()
                //{
                //    ProductID = ProductID,
                //    Quan = Quan,
                //    UserID = Common.Props.LoginUser.UserID,
                //    LogDT = DateTime.Now
                //});
                ////--
                try
                {
                    db.SaveChanges();
                    res.PrimeKeyValue = SaveModel.ProductID;
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

        public SavingResult UpdateRateUplift(int ProductID, decimal RateUplift)
        {
            SavingResult res = new SavingResult();

            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                tblProduct SaveModel = db.tblProducts.Find(ProductID);
                if (SaveModel == null)
                {
                    res.ExecutionResult = eExecutionResult.ValidationError;
                    res.ValidationError = "Product not found";
                    return res;
                }

                SaveModel = db.tblProducts.FirstOrDefault(r => r.ProductID == ProductID);
                if (SaveModel == null)
                {
                    res.ExecutionResult = eExecutionResult.ValidationError;
                    res.ValidationError = "Selected user has been deleted over network. Can not find product details. Please retry.";
                    return res;
                }

                if (SaveModel.RateUpliftPerc == RateUplift) // if quantity has not changed then don't update quantity.
                {
                    return new SavingResult()
                    {
                        ExecutionResult = eExecutionResult.NotExecutedYet
                    };
                }

                SaveModel.RateUpliftPerc = RateUplift;
                SaveModel.redt = DateTime.Now;
                //SaveModel.reuid = Common.Props.LoginUser.UserID;

                db.tblProducts.Attach(SaveModel);
                db.Entry(SaveModel).State = System.Data.Entity.EntityState.Modified;

                //--
                try
                {
                    db.SaveChanges();
                    res.PrimeKeyValue = SaveModel.ProductID;
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

        public SavingResult UpdateRateUpliftAllProducts(decimal RateUplift)
        {
            SavingResult res = new SavingResult();

            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                //if (SaveModel.RateUpliftPerc == RateUplift) // if quantity has not changed then don't update quantity.
                //{
                //    return new SavingResult()
                //    {
                //        ExecutionResult = eExecutionResult.NotExecutedYet
                //    };
                //}

                foreach (tblProduct SaveModel in db.tblProducts)
                {
                    SaveModel.RateUpliftPerc = RateUplift;
                    SaveModel.redt = DateTime.Now;
                    //SaveModel.reuid = Common.Props.LoginUser.UserID;

                    db.tblProducts.Attach(SaveModel);
                    db.Entry(SaveModel).State = System.Data.Entity.EntityState.Modified;
                }
                //--
                try
                {
                    db.SaveChanges();
                    res.PrimeKeyValue = 0;// SaveModel.ProductID;
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
    }
}