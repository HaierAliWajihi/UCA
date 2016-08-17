using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UCAOrderManager.Controllers.Inventory
{
    public class UpdateStockController : Controller
    {
        DAL.Inventory.UpdateStockDAL UpdateStockDALObj = new DAL.Inventory.UpdateStockDAL();
        public ActionResult Index()
        {
            if (Common.Props.LoginUser == null)
            {
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/UpdateStock/Index/" });
            }
            else if (Common.Props.LoginUser != null && Common.Props.LoginUser.Role != Models.Users.eUserRoleID.Admin)
            {
                return RedirectToAction("PermissionDenied", "Home");
            }

            return View(UpdateStockDALObj.GetStockList());
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

            Models.Template.SavingResult res = UpdateStockDALObj.UpdateInventory(ProductID, Quan);
            switch(res.ExecutionResult)
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
	}
}