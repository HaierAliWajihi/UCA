using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UCAOrderManager.DAL.Customer;
using UCAOrderManager.DAL.SaleOrder;
using UCAOrderManager.Models.Customer;
using UCAOrderManager.Models.Product;
using UCAOrderManager.Models.SaleOrder;
using UCAOrderManager.Models.Template;

namespace UCAOrderManager.Controllers.SaleOrder
{
    public class SaleOrderController : Controller
    {
        private SaleOrderDAL DALObj = new SaleOrderDAL();

        public ActionResult Index()
        {
            if (Common.Props.LoginUser == null)
            {
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/SaleOrder/Index" });
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
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/SaleOrder/Details/" + id.Value.ToString() });
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
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/SaleOrder/Create" });
            }
            else if (Common.Props.LoginUser != null && !(Common.Props.LoginUser.Role == Models.Users.eUserRoleID.Admin ||
                Common.Props.LoginUser.Role == Models.Users.eUserRoleID.Customer))
            {
                return RedirectToAction("PermissionDenied", "Home");
            }

            SaleOrderViewModel ViewModel = new SaleOrderViewModel();
            CustomerDAL CustomerDALObj = new CustomerDAL();
            CustomerViewModel CustomerViewModel = CustomerDALObj.FindByID(Common.Props.LoginUser.UserID);
            DAL.Product.ProductMasterDAL ProductDAL = new DAL.Product.ProductMasterDAL();
            ViewModel.SODate = DateTime.Now.Date;

            if (Common.Props.LoginUser.Role != Models.Users.eUserRoleID.Admin)
            {
                ViewModel.CustomerID = CustomerViewModel.UserID;
            }
            ViewModel.BusinessName = CustomerViewModel.BusinessName;
            ViewModel.ContactName = CustomerViewModel.ContactName;
            ViewModel.Address = CustomerViewModel.Address ?? "";
            ViewModel.City = CustomerViewModel.City;
            ViewModel.Postcode = CustomerViewModel.Postcode ?? "";
            ViewModel.Country = CustomerViewModel.Country;
            ViewModel.IntPhoneNo = CustomerViewModel.IntPhoneNo;
            ViewModel.EMailContact = CustomerViewModel.EMailID;
            ViewModel.AirportDestCity = CustomerViewModel.AirportDestCity;
            ViewModel.Products = ProductDAL.GetList().Select<ProductMasterListViewModel, SaleOrderProducDetailViewModel>(r => new SaleOrderProducDetailViewModel()
            {
                ProductID = r.ProductID,
                ScientificName = r.ScientificName,
                CommonName = r.CommonName ,
                Descr = r.Descr,
                SizeName = r.SizeName,
                CultivationTypeName = r.CultivationTypeName,
                Rate = r.Rate,
                CurrentStock = r.CurrentStock
            }).ToList();

            ViewBag.NewSaleOrderCode = 0;
            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SaleOrderID,CustomerID,SODate,SONo,CustomerID,BusinessName,ContactName,Address,City,Postcode,Country,IntPhoneNo,EMailContact,AirportDestCity,EstDelDate,Products")] Models.SaleOrder.SaleOrderViewModel ViewModel)
        {
            if (Common.Props.LoginUser == null)
            {
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/SaleOrder/Create" });
            }
            else if (Common.Props.LoginUser != null && !(Common.Props.LoginUser.Role == Models.Users.eUserRoleID.Admin ||
                Common.Props.LoginUser.Role == Models.Users.eUserRoleID.Customer))
            {
                return RedirectToAction("PermissionDenied", "Home");
            }
            else if(ViewModel.Products.Count(r=> r.OrderQty > 0) == 0)
            {
                ModelState.AddModelError("", "You have not entered any order. Please enter order quantity before proceed.");
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
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/SaleOrder/Edit/" + id.Value.ToString() });
            }
            else if (Common.Props.LoginUser != null && !(Common.Props.LoginUser.Role == Models.Users.eUserRoleID.Admin ||
                Common.Props.LoginUser.Role == Models.Users.eUserRoleID.Customer))
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
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/SaleOrder/Delete/" + id.Value.ToString() });
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
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/SaleOrder/Delete/" + id.ToString() });
            }
            else if (Common.Props.LoginUser != null && !(Common.Props.LoginUser.Role == Models.Users.eUserRoleID.Admin ||
                Common.Props.LoginUser.Role == Models.Users.eUserRoleID.Customer))
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

        public ActionResult GenerateSaleInvoice(int id)
        {
            if (Common.Props.LoginUser == null)
            {
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/SaleOrder/Edit/" + id.ToString() });
            }
            else if (Common.Props.LoginUser != null && !(Common.Props.LoginUser.Role == Models.Users.eUserRoleID.Admin ))
            {
                return RedirectToAction("PermissionDenied", "Home");
            }

            return RedirectToAction("CreateFromOrder", "SaleInvoice", new { SaleOrderID = id});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DALObj = null;
            }
            base.Dispose(disposing);
        }
    }
}