using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCAOrderManager.DAL.SaleInvoice;
using UCAOrderManager.Models.SaleInvoice;

namespace UCAOrderManager.Controllers.SaleInvoice
{
    public class BoxListController : Controller
    {
        private BoxListDAL DALObj = new BoxListDAL();

        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequest", "Home");
            }
            if (Common.Props.LoginUser == null)
            {
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/SaleInvoice/Create" });
            }
            else if (Common.Props.LoginUser != null && !(Common.Props.LoginUser.Role == Models.Users.eUserRoleID.Admin))
            {
                return RedirectToAction("PermissionDenied", "Home");
            }

            SaleInvoiceDAL SIDAL = new SaleInvoiceDAL();
            SaleInvoiceViewModel SaleInvoice = SIDAL.FindByID(id.Value);
            if(SaleInvoice == null)
            {
                return RedirectToAction("RecordNotFound", "Home");
            }

            BoxListViewModel ViewModel = new BoxListViewModel()
            {
                SaleInvoiceID = SaleInvoice.SaleInvoiceID,

                BusinessName = SaleInvoice.BusinessName,
                ContactName = SaleInvoice.ContactName,
                Address = SaleInvoice.Address,
                City = SaleInvoice.City,
                Postcode = SaleInvoice.Postcode,
                Country = SaleInvoice.Country,
                IntPhoneNo = SaleInvoice.IntPhoneNo,
                EMailContact = SaleInvoice.EMailContact,
                AirportDestCity = SaleInvoice.AirportDestCity,
                ShippingDate = SaleInvoice.ShippingDate,
                ArrivalDate = SaleInvoice.ArrivalDate,
                DomesticFlight = SaleInvoice.DomesticFlight,
                InternationalFlight = SaleInvoice.InternationalFlight,

                BoxListDetails = new List<BoxListBoxDetailViewModel>()
                {
                    new BoxListBoxDetailViewModel()
                    {
                        BoxNo = 1,
                        Products = new List<BoxListProductDetailViewModel>()
                        {
                            new BoxListProductDetailViewModel(){},
                        }
                    }
                }
            };
            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BoxListID,SaleInvoiceID,BoxListDetails")] Models.SaleInvoice.BoxListViewModel ViewModel)
        {
            if (Common.Props.LoginUser == null)
            {
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/SaleInvoice/Index" });
            }
            else if (Common.Props.LoginUser != null && !(Common.Props.LoginUser.Role == Models.Users.eUserRoleID.Admin))
            {
                return RedirectToAction("PermissionDenied", "Home");
            }
            //else if (ViewModel.Products.Count(r => r.Qty > 0) == 0)
            //{
            //    ModelState.AddModelError("", "You have not entered any quantity. Please enter quantity before proceed.");
            //}

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

            var ViewModel = DALObj.FindByID(id.Value);
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