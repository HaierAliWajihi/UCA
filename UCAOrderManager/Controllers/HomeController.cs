using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UCAOrderManager.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult PermissionDenied()
        {
            return View();
        }

        public ActionResult BadRequest(string Message)
        {
            ViewBag.Message = Message;
            return View();
        }

        public ActionResult RecordNotFound(string Message)
        {
            ViewBag.Message = Message;
            return View();
        }
    }
}