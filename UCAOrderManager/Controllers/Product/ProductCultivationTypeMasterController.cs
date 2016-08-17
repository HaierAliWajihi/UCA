using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace UCAOrderManager.Controllers.Product
{
    public class ProductCultivationTypeMasterController : Controller
    {
        private DAL.Product.ProductCultivationTypeDAL DALObj = new DAL.Product.ProductCultivationTypeDAL();

        public ActionResult Index()
        {
            if (Common.Props.LoginUser == null)
            {
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/ProductCultivationTypeMaster/Index/" });
            }
            else if (Common.Props.LoginUser != null && Common.Props.LoginUser.Role != Models.Users.eUserRoleID.Admin)
            {
                return RedirectToAction("PermissionDenied", "Home");
            }

            return View(DALObj.GetList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequest", "Home");
            }

            if (Common.Props.LoginUser == null)
            {
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/ProductCultivationTypeMaster/Details/" + id.Value.ToString() });
            }
            else if (Common.Props.LoginUser != null && Common.Props.LoginUser.Role != Models.Users.eUserRoleID.Admin)
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
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/ProductCultivationTypeMaster/Create/" });
            }
            else if (Common.Props.LoginUser != null && Common.Props.LoginUser.Role != Models.Users.eUserRoleID.Admin)
            {
                return RedirectToAction("PermissionDenied", "Home");
            }
            //--
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductCultivationTypeID,ProductCultivationTypeName")] Models.Product.ProductCultivationTypeViewModel ViewModel)
        {

            if (Common.Props.LoginUser == null)
            {
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/ProductCultivationTypeMaster/Create/" });
            }
            else if (Common.Props.LoginUser != null && Common.Props.LoginUser.Role != Models.Users.eUserRoleID.Admin)
            {
                return RedirectToAction("PermissionDenied", "Home");
            }

            if (ModelState.IsValid)
            {
                if (Common.Functions.SetAfterSaveResult(ModelState, DALObj.SaveRecord(ViewModel)))
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
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/ProductCultivationTypeMaster/Edit/" + id.Value.ToString() });
            }
            else if (Common.Props.LoginUser != null && Common.Props.LoginUser.Role != Models.Users.eUserRoleID.Admin)
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
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/ProductCultivationTypeMaster/Delete/" + id.ToString() });
            }
            else if (Common.Props.LoginUser != null && Common.Props.LoginUser.Role != Models.Users.eUserRoleID.Admin)
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
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/ProductCultivationTypeMaster/Delete/" + id.ToString() });
            }
            else if (Common.Props.LoginUser != null && Common.Props.LoginUser.Role != Models.Users.eUserRoleID.Admin)
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