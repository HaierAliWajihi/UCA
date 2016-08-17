using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UCAOrderManager.Models.Template;

namespace UCAOrderManager.DAL.Inventory
{
    public class UpdateStockDAL
    {
        public List<Models.Inventory.UpdateStockViewModel> GetStockList()
        {
            using(dbUltraCoralEntities db = new dbUltraCoralEntities())
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

                        select new Models.Inventory.UpdateStockViewModel()
                        {
                            ProductID = r.ProductID,
                            ProductCode = r.ProductCode,
                            ScientificName = (rct != null ? rsn.ProductScientificName : ""),
                            CommonName = (rcn != null ? rcn.ProductCommonName : ""),
                            Descr = r.Descr,
                            Size = (rsize != null ? rsize.ProductSizeName : ""),
                            CultivationType = (rct != null ? rct.ProductCultivationTypeName : ""),
                            Rate = r.Rate,
                            CurrentStock = (int)r.CurrentStock

                        }).ToList();

            }
        }

        public SavingResult UpdateInventory(int ProductID, decimal Quan)
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

                if(SaveModel.CurrentStock == Quan) // if quantity has not changed then don't update quantity.
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

    }
}