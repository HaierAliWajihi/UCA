using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCAOrderManager.Models.Product;
using UCAOrderManager.Models.SaleInvoice;
using UCAOrderManager.Models.Template;
using UCAReports.SaleInvoice;
using UCAReports.SERLabel;

namespace UCAOrderManager.DAL.SaleInvoice
{
    public class SaleInvoiceDAL
    {
        public List<SaleInvoiceListViewModel> GetList(int? CustomerID)
        {
            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                return (from r in db.tblSaleInvoices
                        join grs in db.tblSaleInvoiceStatus on r.SaleInvoiceStatusID equals grs.SaleInvoiceStatusID into groupRS
                        from rs in groupRS.DefaultIfEmpty()
                        where CustomerID == null || r.CustomerID == CustomerID
                        select new SaleInvoiceListViewModel()
                        {
                            SaleInvoiceID = r.SaleInvoiceID,
                            InvoiceDate = r.InvoiceDate,
                            InvoiceNo = r.InvoiceNo,
                            SaleInvoiceStatusID = r.SaleInvoiceStatusID,
                            SaleInvoiceStatus = (rs != null ? rs.SaleInvoiceStatusName : "None"),
                            CustomerID = r.CustomerID,
                            BusinessName = r.BusinessName,
                            ShippingDate = r.ShippingDate,
                            ArrivalDate = r.ArrivalDate,
                            DomesticFlight = r.DomesticFlight,
                            InternationalFlight = r.InternationalFlight,
                            TotalQty = r.TotalQuan,
                            EstBoxes = r.EstBoxes
                        }).ToList();
            }
        }

        public SaleInvoiceViewModel FindByID(int ID)
        {
            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                var SaveModel = db.tblSaleInvoices.Find(ID);
                if (SaveModel == null)
                {
                    return null;
                }
                else
                {
                    SaleInvoiceViewModel ViewModel = new SaleInvoiceViewModel()
                    {
                        SaleInvoiceID = SaveModel.SaleInvoiceID,
                        InvoiceDate = SaveModel.InvoiceDate,
                        InvoiceNo = SaveModel.InvoiceNo,
                        SaleInvoiceStatusID =SaveModel.SaleInvoiceStatusID,
                        SaleInvoiceStatus = (SaveModel.tblSaleInvoiceStatu != null ? SaveModel.tblSaleInvoiceStatu.SaleInvoiceStatusName : "None"),
                        
                        CustomerID = SaveModel.CustomerID,
                        BusinessName = SaveModel.BusinessName,
                        ContactName = SaveModel.ContactName,
                        Address = SaveModel.Address,
                        City = SaveModel.City,
                        Postcode = SaveModel.Postcode,
                        Country = SaveModel.Country,
                        IntPhoneNo = SaveModel.IntPhoneNo,
                        EMailContact = SaveModel.EmailContact,
                        AirportDestCity = SaveModel.AirportDestCity,
                        ShippingDate = SaveModel.ShippingDate,
                        ArrivalDate = SaveModel.ArrivalDate,
                        DomesticFlight = SaveModel.DomesticFlight,
                        InternationalFlight = SaveModel.InternationalFlight,

                        TotalQuan = SaveModel.TotalQuan,
                        TotalGAmt = SaveModel.TotalGAmt,
                        EstBoxes = SaveModel.EstBoxes,
                        BoxCharges = SaveModel.BoxCharges,
                        DomesticFreightCharges = SaveModel.DomesticFreightCharges,
                        IntFreightCharges = SaveModel.IntFreightCharges,
                        TTFee = SaveModel.TTFee,
                        TotalFreight = SaveModel.TotalFreight,
                        PreviousCredit = SaveModel.PreviousCredit,
                        TotalPayableAmt = SaveModel.TotalPayableAmt,

                        Products = SaveModel.tblSaleInvoiceProductDetails.Select<tblSaleInvoiceProductDetail, SaleInvoiceProducDetailViewModel>(rp=> new SaleInvoiceProducDetailViewModel()
                        {
                            ProductID = rp.ProductID,
                            ScientificName = rp.tblProduct.tblProductScientificName.ProductScientificName,
                            CommonName = rp.tblProduct.tblProductCommonName.ProductCommonName,
                            Descr = rp.tblProduct.Descr,
                            SizeName = rp.tblProduct.tblProductSize.ProductSizeName,
                            //CultivationTypeName = rp.tblProduct.tblProductCultivationType.ProductCultivationTypeName,
                            Rate = rp.Rate,
                            //CurrentStock = rp.tblProduct.CurrentStock,
                            Qty = rp.Quan,
                            Amt = rp.Amt
                        }).ToList()
                    };

                    return ViewModel;
                }
            }
        }

        public SaleInvoiceViewModel FindByIDWithAllProducts(int ID)
        {
            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                var SaveModel = db.tblSaleInvoices.Find(ID);
                if (SaveModel == null)
                {
                    return null;
                }
                else
                {
                    SaleInvoiceViewModel ViewModel = new SaleInvoiceViewModel()
                    {
                        SaleInvoiceID = SaveModel.SaleInvoiceID,
                        InvoiceDate = SaveModel.InvoiceDate,
                        InvoiceNo = SaveModel.InvoiceNo,
                        SaleInvoiceStatusID = SaveModel.SaleInvoiceStatusID,
                        SaleInvoiceStatus = (SaveModel.tblSaleInvoiceStatu != null ? SaveModel.tblSaleInvoiceStatu.SaleInvoiceStatusName : "None"),

                        CustomerID = SaveModel.CustomerID,
                        BusinessName = SaveModel.BusinessName,
                        ContactName = SaveModel.ContactName,
                        Address = SaveModel.Address,
                        City = SaveModel.City,
                        Postcode = SaveModel.Postcode,
                        Country = SaveModel.Country,
                        IntPhoneNo = SaveModel.IntPhoneNo,
                        EMailContact = SaveModel.EmailContact,
                        AirportDestCity = SaveModel.AirportDestCity,
                        ShippingDate = SaveModel.ShippingDate,
                        ArrivalDate = SaveModel.ArrivalDate,
                        DomesticFlight = SaveModel.DomesticFlight,
                        InternationalFlight = SaveModel.InternationalFlight,

                        TotalQuan = SaveModel.TotalQuan,
                        TotalGAmt = SaveModel.TotalGAmt,
                        EstBoxes = SaveModel.EstBoxes,
                        BoxCharges = SaveModel.BoxCharges,
                        DomesticFreightCharges = SaveModel.DomesticFreightCharges,
                        IntFreightCharges = SaveModel.IntFreightCharges,
                        TTFee = SaveModel.TTFee,
                        TotalFreight = SaveModel.TotalFreight,
                        PreviousCredit = SaveModel.PreviousCredit,
                        TotalPayableAmt = SaveModel.TotalPayableAmt,
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
                                          join sipd in
                                              (from saleinvprod in db.tblSaleInvoiceProductDetails where saleinvprod.SaleInvoiceID == ID select saleinvprod)
                                            on r.ProductID equals sipd.ProductID into joinsipd
                                          from rsipd in joinsipd.DefaultIfEmpty()

                                          select new SaleInvoiceProducDetailViewModel()
                                          {
                                              ProductID = r.ProductID,
                                              ScientificName = (rct != null ? rsn.ProductScientificName : ""),
                                              CommonName = (rcn != null ? rcn.ProductCommonName : ""),
                                              Descr = r.Descr,
                                              SizeName = (rsize != null ? rsize.ProductSizeName : ""),
                                              //CultivationTypeName = (rct != null ? rct.ProductCultivationTypeName : ""),
                                              //CurrentStock = (rsopd != null ? SaveModel.CurrentStock : 0),
                                              Rate = (rsipd != null ? rsipd.Rate : 0),
                                              Qty = (rsipd != null ? rsipd.Quan : 0),
                                              Amt = (rsipd != null ? rsipd.Amt : 0),
                                              IsQtyReq = rsize.QuanReq ?? false
                                          }).ToList();

                    return ViewModel;
                }
            }
        }

        public SavingResult SaveRecord(SaleInvoiceViewModel ViewModel)
        {
            SavingResult res = new SavingResult();

            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                if (ViewModel.SaleInvoiceID != 0 && CheckDuplicate(ViewModel.SaleInvoiceID, ViewModel.InvoiceNo))
                {
                    res.ExecutionResult = eExecutionResult.ValidationError;
                    res.ValidationError = "Can not accept duplicate values. The Cultivation Type name is already exists.";
                    return res;
                }
                else
                {
                    //var x = from p in db.tblProducts
                    //        join ps in db.tblProductSizes on p.SizeID equals ps.ProductSizeID
                    //        join vm in ViewModel.Products on p.ProductID equals vm.ProductID
                    //        where (ps.QuanReq ?? false) == true && vm.Qty == 0
                    //        select new { count = 1 };
                    if (ViewModel.Products.Count(r=> r.IsQtyReq && r.Qty == 0) > 0)
                    {
                        res.ExecutionResult = eExecutionResult.ValidationError;
                        res.ValidationError = "Please enter quantity in per plyp and per head size products.";
                        return res;
                    }
                }

                tblSaleInvoice SaveModel = null;
                if (ViewModel.SaleInvoiceID == 0)
                {
                    SaveModel = new tblSaleInvoice()
                    {
                        InvoiceNo = GenerateNextInvoiceNo(db),
                        rcdt = DateTime.Now,
                        rcuid = Common.Props.LoginUser.UserID
                    };
                    db.tblSaleInvoices.Add(SaveModel);

                    //if((ViewModel.SaleOrderID ?? 0) != 0)
                    //{
                    //    tblSaleOrder SaleOrder = db.tblSaleOrders.Find(ViewModel.SaleOrderID);
                    //    SaleOrder.tblSaleInvoice = SaveModel;
                    //    db.tblSaleOrders.Attach(SaleOrder);
                    //    db.Entry(SaleOrder).State = System.Data.Entity.EntityState.Modified;
                    //}
                }
                else
                {
                    SaveModel = db.tblSaleInvoices.FirstOrDefault(r => r.SaleInvoiceID == ViewModel.SaleInvoiceID);
                    if (SaveModel == null)
                    {
                        res.ExecutionResult = eExecutionResult.ValidationError;
                        res.ValidationError = "Selected user has been deleted over network. Can not find user's details. Please retry.";
                        return res;
                    }

                    SaveModel.redt = DateTime.Now;
                    SaveModel.reuid = Common.Props.LoginUser.UserID;

                    db.tblSaleInvoices.Attach(SaveModel);
                    db.Entry(SaveModel).State = System.Data.Entity.EntityState.Modified;

                    foreach (tblSaleInvoiceProductDetail SIPD in SaveModel.tblSaleInvoiceProductDetails)
                    {
                        tblProduct Product = db.tblProducts.Find(SIPD.ProductID);
                        if (Product != null)
                        {
                            Product.CurrentStock += SIPD.Quan;
                            db.tblProducts.Attach(Product);
                            db.Entry(Product).State = System.Data.Entity.EntityState.Modified;
                        }
                    }
                    db.tblSaleInvoiceProductDetails.RemoveRange(db.tblSaleInvoiceProductDetails.Where(r => r.SaleInvoiceID == ViewModel.SaleInvoiceID));
                }

                //SaveModel.SaleOrderID = ViewModel.SaleOrderID;
                SaveModel.InvoiceDate = ViewModel.InvoiceDate;
                SaveModel.InvoiceNo = ViewModel.InvoiceNo;
                SaveModel.SaleInvoiceStatusID = ViewModel.SaleInvoiceStatusID;
                SaveModel.CustomerID = ViewModel.CustomerID;
                SaveModel.BusinessName = ViewModel.BusinessName;
                SaveModel.ContactName = ViewModel.ContactName;
                SaveModel.Address = ViewModel.Address;
                SaveModel.City = ViewModel.City;
                SaveModel.Postcode = ViewModel.Postcode;
                SaveModel.Country = ViewModel.Country;
                SaveModel.IntPhoneNo = ViewModel.IntPhoneNo;
                SaveModel.EmailContact = ViewModel.EMailContact;
                SaveModel.AirportDestCity = ViewModel.AirportDestCity;
                SaveModel.ShippingDate = ViewModel.ShippingDate;
                SaveModel.ArrivalDate = ViewModel.ArrivalDate;
                SaveModel.DomesticFlight = ViewModel.DomesticFlight;
                SaveModel.InternationalFlight = ViewModel.InternationalFlight;
                //SaveModel.EstDelDate = ViewModel.EstDelDate;
                //SaveModel.FreightAWBNo = ViewModel.AWBNo;
                //SaveModel.FreightAWBNo = ViewModel.AWBNo ?? "";
                //SaveModel.FreightFlight1 = ViewModel.Flight1 ?? "";
                //SaveModel.FreightFlight2 = ViewModel.Flight2 ?? "";
                SaveModel.TotalQuan = (ViewModel.Products.Sum(r => (decimal?)r.Qty) ?? 0);
                SaveModel.TotalGAmt = (ViewModel.Products.Sum(r => (decimal?)r.Amt) ?? 0);

                SaveModel.EstBoxes = ViewModel.EstBoxes;
                SaveModel.BoxCharges = ViewModel.BoxCharges;
                SaveModel.DomesticFreightCharges = ViewModel.DomesticFreightCharges;
                SaveModel.IntFreightCharges = ViewModel.IntFreightCharges;
                SaveModel.TTFee = ViewModel.TTFee;
                SaveModel.TotalFreight = ViewModel.TotalFreight;
                SaveModel.PreviousCredit = ViewModel.PreviousCredit;
                SaveModel.TotalPayableAmt = ViewModel.TotalPayableAmt;


                foreach(SaleInvoiceProducDetailViewModel rp in ViewModel.Products.Where(r=> r.Qty != 0))
                {
                    SaveModel.tblSaleInvoiceProductDetails.Add(new tblSaleInvoiceProductDetail()
                    {
                        //tblSaleInvoice = SaveModel,
                        ProductID = rp.ProductID,
                        Rate = rp.Rate,
                        Quan = rp.Qty,
                        Amt = rp.Amt
                    });

                    tblProduct Product = db.tblProducts.Find(rp.ProductID);
                    if (Product != null)
                    {
                        Product.CurrentStock -= rp.Qty;
                        db.tblProducts.Attach(Product);
                        db.Entry(Product).State = System.Data.Entity.EntityState.Modified;
                    }
                }
                
                //--
                try
                {
                    db.SaveChanges();
                    res.PrimeKeyValue = SaveModel.SaleInvoiceID;
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

            //if (db.tblSaleInvoices.FirstOrDefault(r => r.SaleInvoiceID == ID) != null)
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

                var RecordToDelete = db.tblSaleInvoices.Find(ID);

                if (RecordToDelete == null)
                {
                    res.ExecutionResult = eExecutionResult.ValidationError;
                    res.ValidationError = "Selected record is already deleted or changed over network. Record not found.";
                    return res;
                }

                foreach (tblSaleInvoiceProductDetail SIPD in RecordToDelete.tblSaleInvoiceProductDetails)
                {
                    tblProduct Product = db.tblProducts.Find(SIPD.ProductID);
                    if (Product != null)
                    {
                        Product.CurrentStock += SIPD.Quan;
                        db.tblProducts.Attach(Product);
                        db.Entry(Product).State = System.Data.Entity.EntityState.Modified;
                    }
                }
                db.tblSaleInvoiceProductDetails.RemoveRange(db.tblSaleInvoiceProductDetails.Where(r => r.SaleInvoiceID == ID));

                //if (RecordToDelete.SaleOrderID != null && RecordToDelete.SaleOrderID != 0)
                //{
                //    tblSaleOrder SaleOrder = db.tblSaleOrders.Find(RecordToDelete.SaleOrderID);
                //    SaleOrder.tblSaleInvoice = null;
                //    SaleOrder.SaleInvoiceID = null;
                //    db.tblSaleOrders.Attach(SaleOrder);
                //    db.Entry(SaleOrder).State = System.Data.Entity.EntityState.Modified;
                //}

                db.tblSaleInvoices.Remove(RecordToDelete);
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

        public bool CheckDuplicate(int ID, int Invoiceno)
        {
            using (DAL.dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                return CheckDuplicate(ID, Invoiceno, db);
            }
        }
        public bool CheckDuplicate(int ID, int InvoiceNo, dbUltraCoralEntities db)
        {
            return db.tblSaleInvoices.FirstOrDefault(r => r.SaleInvoiceID != ID && r.InvoiceNo == InvoiceNo) != null;
        }

        public static SelectList GetSelectList()
        {
            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                var list = (from r in db.tblSaleInvoices
                        select new SaleInvoiceSelectListViewModel()
                        {
                            SaleInvoiceID = r.SaleInvoiceID,
                            InvoiceNo = r.InvoiceNo,
                            InvoiceDate = r.InvoiceDate
                        }).ToList();

                return new SelectList(list, "SaleInvoiceID", "InvoiceNo");
            }
        }

        public int GenerateNextInvoiceNo()
        {
            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                return GenerateNextInvoiceNo(db);
            }
        }
        public int GenerateNextInvoiceNo(dbUltraCoralEntities db)
        {
            return (db.tblSaleInvoices.Max(r => (int?)r.InvoiceNo) ?? 0) + 1;
        }

        public SaleInvoiceListViewModel FindByIDGetListViewModel(int ID)
        {
            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                var r = db.tblSaleInvoices.Find(ID);
                if (r == null)
                {
                    return null;
                }
                else
                {
                    return new SaleInvoiceListViewModel()
                    {
                        SaleInvoiceID = r.SaleInvoiceID,
                        InvoiceDate = r.InvoiceDate,
                        InvoiceNo = r.InvoiceNo,
                        SaleInvoiceStatusID = r.SaleInvoiceStatusID,
                        SaleInvoiceStatus = (r.tblSaleInvoiceStatu != null ? r.tblSaleInvoiceStatu.SaleInvoiceStatusName : "None"),
                        CustomerID = r.CustomerID,
                        BusinessName = r.BusinessName,
                        ShippingDate = r.ShippingDate,
                        ArrivalDate = r.ArrivalDate,
                        DomesticFlight = r.DomesticFlight,
                        InternationalFlight = r.InternationalFlight,
                        TotalQty = r.TotalQuan,
                        EstBoxes = r.EstBoxes
                    };
                }
            }
        }

        public List<SaleInvoiceReportModel> GetSaleInvoiceReportHeader(int ID)
        {
            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                var SaveModel = db.tblSaleInvoices.Find(ID);
                if (SaveModel == null)
                {
                    return null;
                }
                else
                {
                    SaleInvoiceReportModel ViewModel = new SaleInvoiceReportModel()
                    {
                        SaleInvoiceID = SaveModel.SaleInvoiceID,
                        InvoiceDate = SaveModel.InvoiceDate,
                        InvoiceNo = SaveModel.InvoiceNo,
                        CustomerID = SaveModel.CustomerID,
                        BusinessName = SaveModel.BusinessName,
                        Address = SaveModel.Address,
                        City = SaveModel.City,
                        Postcode = SaveModel.Postcode,
                        Country = SaveModel.Country,
                        ContactName = SaveModel.ContactName,
                        AirportDestCity = SaveModel.AirportDestCity,
                        IntPhoneNo = SaveModel.IntPhoneNo,
                        EmailContact = SaveModel.EmailContact,
                        //EstDelDate = SaveModel.EstDelDate,
                        //AWBNo = SaveModel.FreightAWBNo,
                        //Flight1 = SaveModel.FreightFlight1,
                        //Flight2 = SaveModel.FreightFlight2,

                        TotalQuan = SaveModel.TotalQuan,
                        TotalGAmt = SaveModel.TotalGAmt,
                        DomesticFreightCharges = SaveModel.DomesticFreightCharges,
                        BoxCharges = SaveModel.BoxCharges,
                        IntFreightCharges = SaveModel.IntFreightCharges,
                        TTFee = SaveModel.TTFee,
                        PreviousCredit = SaveModel.PreviousCredit,
                        TotalFreight = SaveModel.TotalFreight,
                        TotalPayableAmt = SaveModel.TotalPayableAmt,
                    };

                    return new List<SaleInvoiceReportModel>() { ViewModel };
                }
            }
        }

        public List<SaleInvoiceProducDetailReportModel> GetSaleInvoiceReportProductDetail(int ID)
        {
            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                var SaveModel = db.tblSaleInvoices.Find(ID);
                if (SaveModel == null)
                {
                    return null;
                }
                else
                {
                    List<SaleInvoiceProducDetailReportModel> ViewModel = SaveModel.tblSaleInvoiceProductDetails.Select<tblSaleInvoiceProductDetail, SaleInvoiceProducDetailReportModel>(rp => new SaleInvoiceProducDetailReportModel()
                    {
                        ProductID = rp.ProductID,
                        ScientificName = rp.tblProduct.tblProductScientificName.ProductScientificName,
                        CommonName = rp.tblProduct.tblProductCommonName.ProductCommonName,
                        Descr = rp.tblProduct.Descr,
                        SizeName = rp.tblProduct.tblProductSize.ProductSizeName,
                        //CultivationTypeName = rp.tblProduct.tblProductCultivationType.ProductCultivationTypeName,
                        Rate = rp.Rate,
                        //CurrentStock = rp.tblProduct.CurrentStock,
                        Qty = rp.Quan,
                        Amt = rp.Amt
                    }).ToList();

                    return ViewModel;
                }
            }
        }

        public List<SERLabelReportModel> GetSERLabelReportData(int ID)
        {
            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                var SaveModel = db.tblSaleInvoices.Find(ID);
                if (SaveModel == null)
                {
                    return null;
                }
                else
                {
                    SERLabelReportModel ViewModel = new SERLabelReportModel()
                    {
                        SaleInvoiceID = SaveModel.SaleInvoiceID,
                        InvoiceDate = SaveModel.InvoiceDate,
                        InvoiceNo = SaveModel.InvoiceNo,
                        CustomerID = SaveModel.CustomerID,
                        BusinessName = SaveModel.BusinessName,
                        Address = SaveModel.Address,
                        City = SaveModel.City,
                        Postcode = SaveModel.Postcode,
                        Country = SaveModel.Country,
                        ContactName = SaveModel.ContactName,
                        AirportDestCity = SaveModel.AirportDestCity,
                        IntPhoneNo = SaveModel.IntPhoneNo,
                        EmailContact = SaveModel.EmailContact,
                        //EstDelDate = SaveModel.EstDelDate,
                        //AWBNo = SaveModel.FreightAWBNo,
                        DomesticFlight = SaveModel.DomesticFlight,
                        InternationalFlight = SaveModel.InternationalFlight,

                    };

                    return new List<SERLabelReportModel>() { ViewModel };
                }
            }
        }
    }
}

