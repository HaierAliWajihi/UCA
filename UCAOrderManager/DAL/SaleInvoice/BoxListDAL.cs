using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UCAOrderManager.Models.SaleInvoice;
using UCAOrderManager.Models.Template;
using System.Data;

namespace UCAOrderManager.DAL.SaleInvoice
{
    public class BoxListDAL
    {
        public BoxListViewModel FindByID(int ID)
        {
            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                var sql = from b in db.tblBoxLists
                          join SI in db.tblSaleInvoices on b.SaleInvoiceID equals SI.SaleInvoiceID
                          join C in db.tblUsers on SI.CustomerID equals C.UserID
                          where b.BoxListID == ID
                          select new { BoxList = b, SaleInvoice = SI, Cust = C };

                var SaveModel = sql.FirstOrDefault();//db.tblBoxLists.Find(ID);
                if (SaveModel == null)
                {
                    return null;
                }
                else
                {
                    BoxListViewModel ViewModel = new BoxListViewModel()
                    {
                        SaleInvoiceID = SaveModel.BoxList.SaleInvoiceID,
                        BoxListID = SaveModel.BoxList.BoxListID,

                        BusinessName = SaveModel.Cust.BusinessName,
                        ContactName = SaveModel.Cust.ContactName,
                        Address = SaveModel.Cust.Address,
                        City = SaveModel.Cust.City,
                        Postcode = SaveModel.Cust.PostCode,
                        Country = SaveModel.Cust.Country,
                        IntPhoneNo = SaveModel.Cust.IntPhoneNo,
                        EMailContact = SaveModel.Cust.EmailID,
                        AirportDestCity = SaveModel.Cust.AirportDestCity,

                        ShippingDate = SaveModel.SaleInvoice.ShippingDate,
                        ArrivalDate = SaveModel.SaleInvoice.ArrivalDate,
                        DomesticFlight = SaveModel.SaleInvoice.DomesticFlight,
                        InternationalFlight = SaveModel.SaleInvoice.InternationalFlight
                    };

                    var BoxDetail = from BoxDet in db.tblBoxListBoxDetails
                                    where BoxDet.BoxListID == ID
                                    select BoxDet;

                    ViewModel.BoxListDetails = BoxDetail.Select<tblBoxListBoxDetail, BoxListBoxDetailViewModel>(rp => new BoxListBoxDetailViewModel()
                        {
                            BoxID = ID,
                            BoxListBoxDetailID = rp.BoxListBoxDetailID,
                            BoxNo = rp.BoxNo,
                            TotalQuan = rp.TotalQuan,

                            Products = db.tblBoxListProductDetails.Where(r => r.BoxListBoxDetailID == rp.BoxListBoxDetailID).Select<tblBoxListProductDetail, BoxListProductDetailViewModel>(r =>

                                new BoxListProductDetailViewModel()
                                {
                                    BoxListBoxDetailID = rp.BoxListBoxDetailID,
                                    BoxListProductDetailID = r.BoxListProductDetailID,
                                    ScientificNameID = r.ProductScientificNameID,
                                    ScientificName = r.tblProductScientificName.ProductScientificName,
                                    Quan = r.Quan
                                }
                            ).ToList()
                        }).ToList();
                    
                    return ViewModel;
                }
            }
        }

        public SavingResult SaveRecord(BoxListViewModel ViewModel)
        {
            SavingResult res = new SavingResult();

            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                //if (ViewModel.SaleInvoiceID != 0 && CheckDuplicate(ViewModel.SaleInvoiceID, ViewModel.InvoiceNo))
                //{
                //    res.ExecutionResult = eExecutionResult.ValidationError;
                //    res.ValidationError = "Can not accept duplicate values. The Cultivation Type name is already exists.";
                //    return res;
                //}
                //else
                //{
                //    //var x = from p in db.tblProducts
                //    //        join ps in db.tblProductSizes on p.SizeID equals ps.ProductSizeID
                //    //        join vm in ViewModel.Products on p.ProductID equals vm.ProductID
                //    //        where (ps.QuanReq ?? false) == true && vm.Qty == 0
                //    //        select new { count = 1 };
                //    if (ViewModel.Products.Count(r => r.IsQtyReq && r.Qty == 0) > 0)
                //    {
                //        res.ExecutionResult = eExecutionResult.ValidationError;
                //        res.ValidationError = "Please enter quantity in per plyp and per head size products.";
                //        return res;
                //    }
                //}

                tblBoxList SaveModel = null;
                if (ViewModel.SaleInvoiceID == 0)
                {
                    SaveModel = new tblBoxList()
                    {
                        rcdt = DateTime.Now,
                        rcuid = Common.Props.LoginUser.UserID
                    };
                    db.tblBoxLists.Add(SaveModel);
                }
                else
                {
                    SaveModel = db.tblBoxLists.FirstOrDefault(r => r.BoxListID == ViewModel.BoxListID);
                    if (SaveModel == null)
                    {
                        res.ExecutionResult = eExecutionResult.ValidationError;
                        res.ValidationError = "Selected user has been deleted over network. Can not find user's details. Please retry.";
                        return res;
                    }

                    SaveModel.redt = DateTime.Now;
                    SaveModel.reuid = Common.Props.LoginUser.UserID;

                    db.tblBoxLists.Attach(SaveModel);
                    db.Entry(SaveModel).State = System.Data.Entity.EntityState.Modified;
                }

                SaveModel.SaleInvoiceID = ViewModel.SaleInvoiceID;

                foreach (BoxListBoxDetailViewModel bd in ViewModel.BoxListDetails)
                {
                    SaveModel.tblBoxListBoxDetails.Add(new tblBoxListBoxDetail()
                    {
                        BoxNo = bd.BoxNo,
                        TotalQuan = bd.TotalQuan,
                        tblBoxListProductDetails = bd.Products.Select<BoxListProductDetailViewModel, tblBoxListProductDetail>(bp => new tblBoxListProductDetail()
                        {
                            ProductScientificNameID = bp.ScientificNameID,
                            Quan = bp.Quan
                        }).ToList()
                    });
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

            //using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            //{
            //    BeforeDeleteValidationResult BeforeDeleteValres = ValidateBeforeDelete(ID, db);
            //    if (!BeforeDeleteValres.IsValidForDelete)
            //    {
            //        res.ExecutionResult = eExecutionResult.ValidationError;
            //        res.ValidationError = "Following errors occured while deleting. " + BeforeDeleteValres.ValidationMessage;
            //        return res;
            //    }

            //    var RecordToDelete = db.tblSaleInvoices.Find(ID);

            //    if (RecordToDelete == null)
            //    {
            //        res.ExecutionResult = eExecutionResult.ValidationError;
            //        res.ValidationError = "Selected record is already deleted or changed over network. Record not found.";
            //        return res;
            //    }

            //    foreach (tblSaleInvoiceProductDetail SIPD in RecordToDelete.tblSaleInvoiceProductDetails)
            //    {
            //        tblProduct Product = db.tblProducts.Find(SIPD.ProductID);
            //        if (Product != null)
            //        {
            //            Product.CurrentStock += SIPD.Quan;
            //            db.tblProducts.Attach(Product);
            //            db.Entry(Product).State = System.Data.Entity.EntityState.Modified;
            //        }
            //    }
            //    db.tblSaleInvoiceProductDetails.RemoveRange(db.tblSaleInvoiceProductDetails.Where(r => r.SaleInvoiceID == ID));

            //    //if (RecordToDelete.SaleOrderID != null && RecordToDelete.SaleOrderID != 0)
            //    //{
            //    //    tblSaleOrder SaleOrder = db.tblSaleOrders.Find(RecordToDelete.SaleOrderID);
            //    //    SaleOrder.tblSaleInvoice = null;
            //    //    SaleOrder.SaleInvoiceID = null;
            //    //    db.tblSaleOrders.Attach(SaleOrder);
            //    //    db.Entry(SaleOrder).State = System.Data.Entity.EntityState.Modified;
            //    //}

            //    db.tblSaleInvoices.Remove(RecordToDelete);
            //    //--
            //    try
            //    {
            //        db.SaveChanges();
            //        res.ExecutionResult = eExecutionResult.CommitedSucessfuly;
            //    }
            //    catch (Exception ex)
            //    {
            //        ex = Common.Functions.FindFinalError(ex);

            //        res.ExecutionResult = eExecutionResult.ErrorWhileExecuting;
            //        res.Exception = ex;
            //    }
            //}
            return res;
        }
    }
}