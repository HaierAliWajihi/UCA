using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCAOrderManager.Models.Product;
using UCAOrderManager.Models.SaleOrder;
using UCAOrderManager.Models.Template;

namespace UCAOrderManager.DAL.SaleOrder
{
    public class SaleOrderDAL
    {
        public List<SaleOrderListViewModel> GetList(int? CustomerID)
        {
            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                return (from r in db.tblSaleOrders
                        where CustomerID == null || r.CustomerID == CustomerID
                        select new SaleOrderListViewModel()
                        {
                            SaleOrderID = r.SaleOrderID,
                            SaleInvoiceID = r.SaleInvoiceID,
                            SODate = r.SODate,
                            SONo = r.SONo,
                            CustomerID = r.CustomerID,
                            BusinessName = r.BusinessName,
                            City = r.City,
                            Country = r.Country,
                            TotalQty = r.TotalQuan,
                            TotalAmt = r.TotalAmt
                        }).ToList();
            }
        }

        public SaleOrderViewModel FindByID(int ID)
        {
            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                var r = db.tblSaleOrders.Find(ID);
                if (r == null)
                {
                    return null;
                }
                else
                {
                    return new SaleOrderViewModel()
                    {
                        SaleOrderID = r.SaleOrderID,
                        SODate = r.SODate,
                        SONo = r.SONo,
                        CustomerID = r.CustomerID,
                        BusinessName = r.BusinessName,
                        Address = r.Address,
                        City = r.City,
                        Postcode = r.Postcode,
                        Country = r.Country,
                        ContactName = r.ContactName,
                        AirportDestCity = r.AirportDestCity,
                        EMailContact = r.EmailContact,
                        IntPhoneNo = r.IntPhoneNo,
                        EstDelDate = r.EstDelDate,
                        Products = r.tblSaleOrderProductDetails.Select<tblSaleOrderProductDetail, SaleOrderProducDetailViewModel>(rp=> new SaleOrderProducDetailViewModel()
                        {
                            ProductID = rp.ProductID,
                            ScientificName = rp.tblProduct.tblProductScientificName.ProductScientificName,
                            CommonName = rp.tblProduct.tblProductCommonName.ProductCommonName,
                            Descr = rp.tblProduct.Descr,
                            SizeName = rp.tblProduct.tblProductSize.ProductSizeName,
                            CultivationTypeName = rp.tblProduct.tblProductCultivationType.ProductCultivationTypeName,
                            Rate = rp.Rate,
                            CurrentStock = rp.tblProduct.CurrentStock,
                            OrderQty = rp.Quan
                        }).ToList()
                    };
                }
            }
        }

        public SaleOrderViewModel FindByIDWithAllProducts(int ID)
        {
            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                var SO = db.tblSaleOrders.Find(ID);
                if (SO == null)
                {
                    return null;
                }
                else
                {
                    SaleOrderViewModel ViewModel = new SaleOrderViewModel()
                    {
                        SaleOrderID = SO.SaleOrderID,
                        SODate = SO.SODate,
                        SONo = SO.SONo,
                        CustomerID = SO.CustomerID,
                        BusinessName = SO.BusinessName,
                        ContactName = SO.ContactName,
                        Address = SO.Address,
                        City = SO.City,
                        Postcode = SO.Postcode,
                        Country = SO.Country,
                        AirportDestCity = SO.AirportDestCity,
                        IntPhoneNo = SO.IntPhoneNo,
                        EstDelDate = SO.EstDelDate,
                    };

                    ViewModel.Products = (from r in db.tblProducts
                                          join sn in db.tblProductScientificNames on r.ScientificNameID equals sn.ProductScientificNameID into joinsn
                                          from rsn in joinsn.DefaultIfEmpty()
                                          join cn in db.tblProductCommonNames on r.CommonNameID equals cn.ProductCommonNameID into joincn
                                          from rcn in joincn.DefaultIfEmpty()
                                          join size in db.tblProductSizes on r.SizeID equals size.ProductSizeID into joinsize
                                          from rsize in joinsize.DefaultIfEmpty()
                                          join ct in db.tblProductCultivationTypes on r.CultivationTypeID equals ct.ProductCultivationTypeID into joinct
                                          from rct in joinct.DefaultIfEmpty()
                                          join sopd in 
                                            (from saleordprod in db.tblSaleOrderProductDetails where saleordprod.SaleOrderID == ID select saleordprod) 
                                            on r.ProductID equals sopd.ProductID into joinsopd
                                          from rsopd in joinsopd.DefaultIfEmpty()

                                          select new SaleOrderProducDetailViewModel()
                                          {
                                              ProductID = r.ProductID,
                                              ScientificName = (rct != null ? rsn.ProductScientificName : ""),
                                              CommonName = (rcn != null ? rcn.ProductCommonName : ""),
                                              Descr = r.Descr,
                                              SizeName = (rsize != null ? rsize.ProductSizeName : ""),
                                              CultivationTypeName = (rct != null ? rct.ProductCultivationTypeName : ""),
                                              CurrentStock = (rsopd != null ? r.CurrentStock : 0),
                                              Rate = (rsopd != null ? rsopd.Rate : 0),
                                              OrderQty = (rsopd != null ? rsopd.Quan : 0)
                                          }).ToList();
                    return ViewModel;
                }
            }
        }

        public SavingResult SaveRecord(SaleOrderViewModel ViewModel)
        {
            SavingResult res = new SavingResult();

            //if (String.IsNullOrWhiteSpace(ViewModel.SaleOrderName))
            //{
            //    res.ExecutionResult = eExecutionResult.ValidationError;
            //    res.ValidationError = "Please enter Cultivation Type name";
            //    return res;
            //}

            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                if (ViewModel.SaleOrderID != 0 && CheckDuplicate(ViewModel.SaleOrderID, ViewModel.SONo))
                {
                    res.ExecutionResult = eExecutionResult.ValidationError;
                    res.ValidationError = "Can not accept duplicate values. The Cultivation Type name is already exists.";
                    return res;
                }

                tblSaleOrder SaveModel = null;
                if (ViewModel.SaleOrderID == 0)
                {
                    SaveModel = new tblSaleOrder()
                    {
                        SONo = GenerateNextSONo(db),
                        rcdt = DateTime.Now,
                        rcuid = Common.Props.LoginUser.UserID
                    };
                    db.tblSaleOrders.Add(SaveModel);
                }
                else
                {
                    SaveModel = db.tblSaleOrders.FirstOrDefault(r => r.SaleOrderID == ViewModel.SaleOrderID);
                    if (SaveModel == null)
                    {
                        res.ExecutionResult = eExecutionResult.ValidationError;
                        res.ValidationError = "Selected user has been deleted over network. Can not find user's details. Please retry.";
                        return res;
                    }

                    SaveModel.redt = DateTime.Now;
                    SaveModel.reuid = Common.Props.LoginUser.UserID;

                    db.tblSaleOrders.Attach(SaveModel);
                    db.Entry(SaveModel).State = System.Data.Entity.EntityState.Modified;

                    db.tblSaleOrderProductDetails.RemoveRange(db.tblSaleOrderProductDetails.Where(r => r.SaleOrderID == ViewModel.SaleOrderID));
                }

                SaveModel.SODate = ViewModel.SODate;
                SaveModel.CustomerID = ViewModel.CustomerID;
                SaveModel.BusinessName = ViewModel.BusinessName;
                SaveModel.ContactName = ViewModel.ContactName;
                SaveModel.Address = ViewModel.Address;
                SaveModel.City = ViewModel.City;
                SaveModel.Postcode = ViewModel.Postcode;
                SaveModel.Country = ViewModel.Country;
                SaveModel.IntPhoneNo = ViewModel.IntPhoneNo;
                SaveModel.AirportDestCity = ViewModel.AirportDestCity;
                SaveModel.EstDelDate = ViewModel.EstDelDate;
                SaveModel.TotalQuan = (ViewModel.Products.Sum(r => (decimal?)r.OrderQty) ?? 0);
                SaveModel.TotalAmt = (ViewModel.Products.Sum(r => (decimal?)(r.OrderQty * r.Rate)) ?? 0);

                foreach(SaleOrderProducDetailViewModel rp in ViewModel.Products.Where(r=> r.OrderQty != 0))
                {
                    SaveModel.tblSaleOrderProductDetails.Add(new tblSaleOrderProductDetail()
                    {
                        tblSaleOrder = SaveModel,
                        ProductID = rp.ProductID,
                        Rate = rp.Rate,
                        Quan = rp.OrderQty,
                        Amt = rp.Rate * rp.OrderQty
                    });
                }
                
                //--
                try
                {
                    db.SaveChanges();
                    res.PrimeKeyValue = SaveModel.SaleOrderID;
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

            //if (db.tblSaleInvoices.FirstOrDefault(r => r.SaleOrderID == ID) != null)
            //{
            //    res.IsValidForDelete = false;
            //    res.ValidationMessage = "Already selected in invoice.";
            //}

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

                var RecordToDelete = db.tblSaleOrders.Find(ID);

                if (RecordToDelete == null)
                {
                    res.ExecutionResult = eExecutionResult.ValidationError;
                    res.ValidationError = "Selected record is already deleted or changed over network. Record not found.";
                    return res;
                }

                db.tblSaleOrderProductDetails.RemoveRange(db.tblSaleOrderProductDetails.Where(r => r.SaleOrderID == ID));
                db.tblSaleOrders.Remove(RecordToDelete);
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

        public bool CheckDuplicate(int ID, int SONo)
        {
            using (DAL.dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                return CheckDuplicate(ID, SONo, db);
            }
        }
        public bool CheckDuplicate(int ID, int SONo, dbUltraCoralEntities db)
        {
            return db.tblSaleOrders.FirstOrDefault(r => r.SaleOrderID != ID && r.SONo == SONo) != null;
        }

        public static SelectList GetSelectList()
        {
            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                var list = (from r in db.tblSaleOrders
                        select new SaleOrderSelectListViewModel()
                        {
                            SaleOrderID = r.SaleOrderID,
                            SONo = r.SONo,
                            SODate = r.SODate
                        }).ToList();

                return new SelectList(list, "SaleOrderID", "SONo");
            }
        }

        public int GenerateNextSONo()
        {
            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                return GenerateNextSONo(db);
            }
        }
        public int GenerateNextSONo(dbUltraCoralEntities db)
        {
            return (db.tblSaleOrders.Max(r => (int?)r.SONo) ?? 0) + 1;

        }

        public SaleOrderListViewModel FindByIDGetListViewModel(int ID)
        {
            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                var r = db.tblSaleOrders.Find(ID);
                if (r == null)
                {
                    return null;
                }
                else
                {
                    return new SaleOrderListViewModel()
                    {
                        SaleOrderID = r.SaleOrderID,
                        CustomerID = r.CustomerID,
                        BusinessName = r.BusinessName,
                        City = r.City,
                        Country = r.Country,
                        TotalQty = r.TotalQuan,
                        TotalAmt = r.TotalAmt
                    };
                }
            }
        }
    }
}