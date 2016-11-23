using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UCAOrderManager.DAL.Customer;
using UCAOrderManager.DAL.SaleInvoice;
using UCAOrderManager.Models.Customer;
using UCAOrderManager.Models.Product;
using UCAOrderManager.Models.SaleInvoice;
using UCAOrderManager.Models.Template;

namespace UCAOrderManager.Controllers.SaleInvoice
{
    public class SaleInvoiceController : Controller
    {
        private SaleInvoiceDAL DALObj = new SaleInvoiceDAL();

        public ActionResult Index()
        {
            if (Common.Props.LoginUser == null)
            {
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/SaleInvoice/Index" });
            }
            else if (Common.Props.LoginUser != null && !(Common.Props.LoginUser.Role == Models.Users.eUserRoleID.Admin || 
                Common.Props.LoginUser.Role == Models.Users.eUserRoleID.Customer))
            {
                return RedirectToAction("PermissionDenied", "Home");
            }

            return View(DALObj.GetList( (Common.Props.LoginUser.Role == Models.Users.eUserRoleID.Customer? (int?)Common.Props.LoginUser.UserID : null)));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequest", "Home");
            }

            if (Common.Props.LoginUser == null)
            {
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/SaleInvoice/Details/" + id.Value.ToString() });
            }
            else if (Common.Props.LoginUser != null && !(Common.Props.LoginUser.Role == Models.Users.eUserRoleID.Admin ||
                Common.Props.LoginUser.Role == Models.Users.eUserRoleID.Customer))
            {
                return RedirectToAction("PermissionDenied", "Home");
            }

            var ViewModel = DALObj.FindByID(id.Value);
            if (ViewModel == null)
            {
                return RedirectToAction("RecordNotFound", "Home");
            }
            return View(ViewModel);
        }

        public ActionResult Create()
        {
            if (Common.Props.LoginUser == null)
            {
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/SaleInvoice/Create" });
            }
            else if (Common.Props.LoginUser != null && !(Common.Props.LoginUser.Role == Models.Users.eUserRoleID.Admin))
            {
                return RedirectToAction("PermissionDenied", "Home");
            }

            SaleInvoiceViewModel ViewModel = new SaleInvoiceViewModel();

            DAL.Product.ProductMasterDAL ProductDAL = new DAL.Product.ProductMasterDAL();
            ViewModel.InvoiceNo = DALObj.GenerateNextInvoiceNo();
            ViewModel.InvoiceDate = DateTime.Now.Date;
            ViewModel.SaleInvoiceStatusID = 1;
            ViewModel.SaleInvoiceStatus = "PI Issued";

            ViewModel.Products = ProductDAL.GetList().Select<ProductMasterListViewModel, SaleInvoiceProducDetailViewModel>(r => new SaleInvoiceProducDetailViewModel()
            {
                ProductID = r.ProductID,
                ScientificName = r.ScientificName,
                CommonName = r.CommonName ,
                Descr = r.Descr,
                SizeName = r.SizeName,
                //CultivationTypeName = r.CultivationTypeName,
                Rate = r.Rate,
                //CurrentStock = r.CurrentStock
                IsQtyReq = r.IsQtyReq
            }).ToList();

            ViewBag.NewSaleInvoiceCode = 0;
            return View(ViewModel);
        }

        //public ActionResult CreateFromOrder(int SaleOrderID)
        //{
        //    //if (SaleOrderID == null)
        //    //{
        //    //    return RedirectToAction("BadRequest", "Home");
        //    //}
        //    if (Common.Props.LoginUser == null)
        //    {
        //        return RedirectToAction("Login", "Users", new { ReturnUrl = "/SaleInvoice/Create" });
        //    }
        //    else if (Common.Props.LoginUser != null && !(Common.Props.LoginUser.Role == Models.Users.eUserRoleID.Admin))
        //    {
        //        return RedirectToAction("PermissionDenied", "Home");
        //    }
            
        //    SaleInvoiceViewModel ViewModel = new SaleInvoiceViewModel();
        //    DAL.SaleOrder.SaleOrderDAL SaleOrderDALObj = new DAL.SaleOrder.SaleOrderDAL();
        //    Models.SaleOrder.SaleOrderViewModel SaleOrder = SaleOrderDALObj.FindByIDWithAllProducts(SaleOrderID);
        //    if(SaleOrder == null)
        //    {
        //        return RedirectToAction("BadRequest", "Home");
        //    }

        //    CustomerDAL CustomerDALObj = new CustomerDAL();
        //    DAL.Product.ProductMasterDAL ProductDAL = new DAL.Product.ProductMasterDAL();
        //    ViewModel.SaleOrderID = SaleOrderID;
        //    ViewModel.InvoiceDate = DateTime.Now.Date;
        //    ViewModel.InvoiceNo = DALObj.GenerateNextInvoiceNo();
        //    ViewModel.CustomerID = SaleOrder.CustomerID;
        //    ViewModel.BusinessName = SaleOrder.BusinessName;
        //    ViewModel.ContactName = SaleOrder.ContactName;
        //    ViewModel.Address = SaleOrder.Address ?? "";
        //    ViewModel.City = SaleOrder.City;
        //    ViewModel.Postcode = SaleOrder.Postcode ?? "";
        //    ViewModel.Country = SaleOrder.Country;
        //    ViewModel.IntPhoneNo = SaleOrder.IntPhoneNo;
        //    ViewModel.EMailContact = SaleOrder.EMailContact ?? "";
        //    ViewModel.AirportDestCity = SaleOrder.AirportDestCity;
        //    //ViewModel.EstDelDate = SaleOrder.EstDelDate;

        //    ViewModel.Products = SaleOrder.Products.Select<Models.SaleOrder.SaleOrderProducDetailViewModel, SaleInvoiceProducDetailViewModel>(r => new SaleInvoiceProducDetailViewModel()
        //    {
        //        ProductID = r.ProductID,
        //        ScientificName = r.ScientificName,
        //        CommonName = r.CommonName,
        //        Descr = r.Descr,
        //        SizeName = r.SizeName,
        //        Qty = r.OrderQty,
        //        Rate = r.Rate,
        //        Amt = Math.Round(r.OrderQty * r.Rate, 2)
        //    }).ToList();

        //    ViewModel.TotalQuan = (ViewModel.Products.Sum(r => (decimal?)r.Qty) ?? 0);
        //    ViewModel.TotalGAmt = (ViewModel.Products.Sum(r => (decimal?)r.Amt) ?? 0);

        //    //ViewModel.DomesticFreightCharges = SaleOrder.DomesticFreightCharges;
        //    //ViewModel.BoxCharges = SaleOrder.BoxCharges;
        //    //ViewModel.IntFreightCharges = SaleOrder.IntFreightCharges;
        //    //ViewModel.TTFee = SaleOrder.TTFee;
        //    //ViewModel.TotalFreight = SaleOrder.TotalFreight;
        //    //ViewModel.PreviousCredit = SaleOrder.PreviousCredit;
        //    ViewModel.TotalPayableAmt = ViewModel.TotalGAmt;

        //    return View("Create", ViewModel);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SaleInvoiceID,InvoiceDate,InvoiceNo,SaleInvoiceStatusID,CustomerID,BusinessName,ContactName,Address,City,Postcode,Country,IntPhoneNo,EMailContact,AirportDestCity,ShippingDate,ArrivalDate,DomesticFlight,InternationalFlight,Products,TotalQuan,TotalGAmt,EstBoxes,BoxCharges,DomesticFreightCharges,IntFreightCharges,TTFee,TotalFreight,PreviousCredit,TotalPayableAmt")] Models.SaleInvoice.SaleInvoiceViewModel ViewModel)
        {
            if (Common.Props.LoginUser == null)
            {
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/SaleInvoice/Create" });
            }
            else if (Common.Props.LoginUser != null && !(Common.Props.LoginUser.Role == Models.Users.eUserRoleID.Admin))
            {
                return RedirectToAction("PermissionDenied", "Home");
            }
            else if(ViewModel.Products.Count(r=> r.Qty > 0) == 0)
            {
                ModelState.AddModelError("", "You have not entered any quantity. Please enter quantity before proceed.");
            }

            if (ModelState.IsValid)
            {
                Models.Template.SavingResult SavingRes = DALObj.SaveRecord(ViewModel);
                if (Common.Functions.SetAfterSaveResult(ModelState, SavingRes))
                {
                    return RedirectToAction("Index");
                }
            }
            return View(ViewModel);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequest", "Home");
            }

            if (Common.Props.LoginUser == null)
            {
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/SaleInvoice/Edit/" + id.Value.ToString() });
            }
            else if (Common.Props.LoginUser != null && !(Common.Props.LoginUser.Role == Models.Users.eUserRoleID.Admin))
            {
                return RedirectToAction("PermissionDenied", "Home");
            }

            var ViewModel = DALObj.FindByIDWithAllProducts(id.Value);
            if (ViewModel == null)
            {
                return RedirectToAction("RecordNotFound", "Home");
            }

            return View("Create", ViewModel);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequest", "Home");
            }

            if (Common.Props.LoginUser == null)
            {
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/SaleInvoice/Delete/" + id.Value.ToString() });
            }
            else if (Common.Props.LoginUser != null && !(Common.Props.LoginUser.Role == Models.Users.eUserRoleID.Admin))
            {
                return RedirectToAction("PermissionDenied", "Home");
            }


            var ViewModel = DALObj.FindByID(id.Value);
            if (ViewModel == null)
            {
                return RedirectToAction("RecordNotFound", "Home");
            }

            Models.Template.BeforeDeleteValidationResult res = DALObj.ValidateBeforeDelete(id.Value);
            if (!res.IsValidForDelete)
            {
                ModelState.AddModelError("", "Can not delete this record. " + res.ValidationMessage);
            }
            ViewBag.CanDelete = res.IsValidForDelete;


            return View(ViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Common.Props.LoginUser == null)
            {
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/SaleInvoice/Delete/" + id.ToString() });
            }
            else if (Common.Props.LoginUser != null && !(Common.Props.LoginUser.Role == Models.Users.eUserRoleID.Admin))
            {
                return RedirectToAction("PermissionDenied", "Home");
            }

            if (Common.Functions.SetAfterSaveResult(ModelState, DALObj.DeleteRecord(id)))
            {
                return RedirectToAction("Index");
            }

            var ViewModel = DALObj.FindByID(id);
            if (ViewModel == null)
            {
                return RedirectToAction("RecordNotFound", "Home");
            }

            return View("Delete", ViewModel);
        }

        public void UpdateTotal()
        {
             
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DALObj = null;
            }
            base.Dispose(disposing);
        }

        public ActionResult SaleInvoicePrint(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequest", "Home");
            }

            if (Common.Props.LoginUser == null)
            {
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/SaleInvoice" });
            }
            else if (Common.Props.LoginUser != null && !(Common.Props.LoginUser.Role == Models.Users.eUserRoleID.Admin || Common.Props.LoginUser.Role == Models.Users.eUserRoleID.Customer))
            {
                return RedirectToAction("PermissionDenied", "Home");
            }

            //var ViewModel = DALObj.FindByIDWithAllProducts(id.Value);
            //if (ViewModel == null)
            //{
            //    return RedirectToAction("RecordNotFound", "Home");
            //}

            //return Redirect("~/Report/Sale/SaleInvoice.aspx?ID" + id.ToString());
            ViewBag.id = id;
            ViewBag.Title = "Full Invoice Print";
            return View();
        }

        public ActionResult SaleInvoicePrintShipping(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequest", "Home");
            }

            if (Common.Props.LoginUser == null)
            {
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/SaleInvoice" });
            }
            else if (Common.Props.LoginUser != null && !(Common.Props.LoginUser.Role == Models.Users.eUserRoleID.Admin || Common.Props.LoginUser.Role == Models.Users.eUserRoleID.Customer))
            {
                return RedirectToAction("PermissionDenied", "Home");
            }

            //var ViewModel = DALObj.FindByIDWithAllProducts(id.Value);
            //if (ViewModel == null)
            //{
            //    return RedirectToAction("RecordNotFound", "Home");
            //}

            //return Redirect("~/Report/Sale/SaleInvoice.aspx?ID" + id.ToString());
            ViewBag.id = id;
            ViewBag.Title = "Shipping Invoice Print";
            return View();
        }

        public ActionResult SaleInvoicePrintSERLabel(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequest", "Home");
            }

            if (Common.Props.LoginUser == null)
            {
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/SaleInvoice" });
            }
            else if (Common.Props.LoginUser != null && !(Common.Props.LoginUser.Role == Models.Users.eUserRoleID.Admin || Common.Props.LoginUser.Role == Models.Users.eUserRoleID.Customer))
            {
                return RedirectToAction("PermissionDenied", "Home");
            }

            ViewBag.id = id;
            ViewBag.Title = "SER Label Print";
            return View();
        }
    }
}