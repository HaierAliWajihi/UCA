using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UCAOrderManager.DAL.Product;
using UCAOrderManager.Models.Product;

namespace UCAOrderManager.Controllers.Product
{
    public class ProductMasterController : Controller
    {
        private ProductMasterDAL DALObj = new ProductMasterDAL();
        private ProductScientificNameDAL ScientificNameDALObj = new ProductScientificNameDAL();
        private ProductCommonNameDAL CommonNameDALObj = new ProductCommonNameDAL();
        private ProductCultivationTypeDAL CultivationTypeDALObj = new ProductCultivationTypeDAL();

        public ActionResult Index()
        {
            if(Common.Props.LoginUser == null)
            {
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/ProductMaster/Index"});
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
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/ProductMaster/Details/" + id.Value.ToString() });
            }
            else if (Common.Props.LoginUser != null && Common.Props.LoginUser.Role != Models.Users.eUserRoleID.Admin)
            {
                return RedirectToAction("PermissionDenied", "Home");
            }

            var ViewModel = DALObj.FindByIDGetListViewModel(id.Value);
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
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/ProductMaster/Create" });
            }
            else if (Common.Props.LoginUser != null && Common.Props.LoginUser.Role != Models.Users.eUserRoleID.Admin)
            {
                return RedirectToAction("PermissionDenied", "Home");
            }

            if (Common.Props.LoginUser == null || Common.Props.LoginUser.Role != Models.Users.eUserRoleID.Admin)
            {
                return RedirectToAction("PermissionDenied", "Home");
            }
            ViewBag.NewProductCode = 0;
            return View(new ProductMasterViewModel() { ProductCode = DALObj.GenerateNewProductCode() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductCode,ScientificNameID,CommonNameID,Descr,SizeID,CultivationTypeID,Rate,RateUplift,CurrentStock")] Models.Product.ProductMasterViewModel ViewModel)
        {
            if (Common.Props.LoginUser == null)
            {
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/ProductMaster/Create" });
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
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/ProductMaster/Edit/" + id.Value.ToString() });
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
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/ProductMaster/Delete/" + id.Value.ToString() });
            }
            else if (Common.Props.LoginUser != null && Common.Props.LoginUser.Role != Models.Users.eUserRoleID.Admin)
            {
                return RedirectToAction("PermissionDenied", "Home");
            }

            
            var ViewModel = DALObj.FindByIDGetListViewModel(id.Value);
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
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/ProductMaster/Delete/" + id.ToString() });
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


        [HttpPost]
        public ActionResult UpdateStock(int ProductID, int Quan)//(string ProductID, string Quan)
        {
            if (Common.Props.LoginUser == null)
            {
                return Json(new { Response = "Please login" });
            }
            else if (Common.Props.LoginUser != null && Common.Props.LoginUser.Role != Models.Users.eUserRoleID.Admin)
            {
                return Json(new { Response = "You don't have permission to update stock." });
            }

            Models.Template.SavingResult res = DALObj.UpdateCurrentStock(ProductID, Quan);
            switch (res.ExecutionResult)
            {
                case Models.Template.eExecutionResult.CommitedSucessfuly:
                    return Json(new { Response = "Saved" });
                case Models.Template.eExecutionResult.ErrorWhileExecuting:
                    return Json(new { Response = "Exception : " + res.Exception.Message });
                case Models.Template.eExecutionResult.ValidationError:
                    return Json(new { Response = "Validaiton Error : " + res.ValidationError });
                default:
                    return Json(new { Response = "" });
            }
            //return View();
        }

        [HttpPost]
        public ActionResult UpdateRate(int ProductID, decimal Rate)
        {
            if (Common.Props.LoginUser == null)
            {
                return Json(new { Response = "Please login" });
            }
            else if (Common.Props.LoginUser != null && Common.Props.LoginUser.Role != Models.Users.eUserRoleID.Admin)
            {
                return Json(new { Response = "You don't have permission to update stock." });
            }

            Models.Template.SavingResult res = DALObj.UpdateRate(ProductID, Rate);
            switch (res.ExecutionResult)
            {
                case Models.Template.eExecutionResult.CommitedSucessfuly:
                    return Json(new { Response = "Saved" });
                case Models.Template.eExecutionResult.ErrorWhileExecuting:
                    return Json(new { Response = "Exception : " + res.Exception.Message });
                case Models.Template.eExecutionResult.ValidationError:
                    return Json(new { Response = "Validaiton Error : " + res.ValidationError });
                default:
                    return Json(new { Response = "" });
            }
            //return View();
        }

        [HttpPost]
        public ActionResult UpdateRateUplift(int ProductID, decimal RateUplift)//(string ProductID, string Quan)
        {
            if (Common.Props.LoginUser == null)
            {
                return Json(new { Response = "Please login" });
            }
            else if (Common.Props.LoginUser != null && Common.Props.LoginUser.Role != Models.Users.eUserRoleID.Admin)
            {
                return Json(new { Response = "You don't have permission to update rate uplift percentage." });
            }

            Models.Template.SavingResult res = DALObj.UpdateRateUplift(ProductID, RateUplift);
            switch (res.ExecutionResult)
            {
                case Models.Template.eExecutionResult.CommitedSucessfuly:
                    return Json(new { Response = "Saved" });
                case Models.Template.eExecutionResult.ErrorWhileExecuting:
                    return Json(new { Response = "Exception : " + res.Exception.Message });
                case Models.Template.eExecutionResult.ValidationError:
                    return Json(new { Response = "Validaiton Error : " + res.ValidationError });
                default:
                    return Json(new { Response = "" });
            }
            //return View();
        }
        
        [HttpPost]
        public ActionResult UpdateRateUpliftAllProducts(decimal? RateUplift)//(string ProductID, string Quan)
        {
            if (Common.Props.LoginUser == null)
            {
                return Json(new { Response = "Please login" });
            }
            else if (Common.Props.LoginUser != null && Common.Props.LoginUser.Role != Models.Users.eUserRoleID.Admin)
            {
                return Json(new { Response = "You don't have permission to update rate uplift percentage." });
            }

            if (RateUplift != null)
            {
                if (Common.Functions.SetAfterSaveResult(ModelState, DALObj.UpdateRateUpliftAllProducts(RateUplift.Value)))
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
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