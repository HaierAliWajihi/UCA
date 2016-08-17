using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UCAOrderManager.Controllers.Company
{
    public class CompanyController : Controller
    {
        public ActionResult Manage()
        {
            return View(Common.Props.CompanyProfile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(Models.Company.CompanyViewModel ViewModel)
        {
            if(ModelState.IsValid)
            {
                DAL.Company.CompanyDAL CompanyDALObj = new DAL.Company.CompanyDAL();
                if(Common.Functions.SetAfterSaveResult(ModelState, CompanyDALObj.SaveRecord(ViewModel)))
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(ViewModel);
        }
    }
}