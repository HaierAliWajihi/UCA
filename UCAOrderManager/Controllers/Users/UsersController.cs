using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using UCAOrderManager.Models.Users;

namespace UCAOrderManager.Controllers.Users
{
    public class UsersController : Controller
    {
        //
        // GET: /Users/
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserRegistrationViewModel ViewModel)
        {
            if(ModelState.IsValid)
            {
                DAL.Users.UserDAL UDAL = new DAL.Users.UserDAL();
                if(Common.Functions.SetAfterSaveResult(ModelState, UDAL.SaveRecord(ViewModel)))
                {
                    // Successfully Saved.
                    //return RedirectToAction("Index","Home");
                    return RedirectToAction("Register", "Users");
                }
            }
            return View(ViewModel);
        }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserLoginViewModel ViewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                DAL.Users.UserDAL UserDALObj = new DAL.Users.UserDAL();
                //var user = await UserManager.FindAsync(model.UserName, model.Password);
                UserLoginDetails User = UserDALObj.MatchUserCredentials(ViewModel.EmailID, ViewModel.Password); //Membership.ValidateUser(model.UserName, model.Password);
                if (User != null)
                {
                    if (User.IsApproved == null)
                    {
                        ModelState.AddModelError("", "Your account approval is still pending. Please contact to your admin for instant approval.");
                    }
                    else if(!User.IsApproved.Value)
                    {
                        ModelState.AddModelError("", "Your account hase been rejected. Please contact customer support for more information.");
                    }
                    else
                    {
                        Common.Props.LoginUser = User;

                        JavaScriptSerializer js = new JavaScriptSerializer();
                        string data = js.Serialize(User);
                        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, User.FullName, DateTime.Now, DateTime.Now.AddMonths(1), ViewModel.RememberMe, data);
                        string encToken = FormsAuthentication.Encrypt(ticket);
                        HttpCookie authoCookies = new HttpCookie(FormsAuthentication.FormsCookieName, encToken);
                        Response.Cookies.Add(authoCookies);
                        //await SignInAsync(user, model.RememberMe);
                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            // If we got this far, something failed, redisplay form
            ModelState.Remove("Password");
            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Common.Props.LoginUser = null;

            FormsAuthentication.SignOut();
            Session.Abandon();

            // clear authentication cookie
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);

            // clear session cookie (not necessary for your current problem but i would recommend you do it anyway)
            HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
            cookie2.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie2);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Manage()
        {
            if(Common.Props.LoginUser == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(Common.Props.LoginUser);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel ViewModel)
        {
            if(ModelState.IsValid)
            {
                DAL.Users.UserDAL UserDALObj = new DAL.Users.UserDAL();

                if(Common.Functions.SetAfterSaveResult(ModelState, UserDALObj.ChangePassword(Common.Props.LoginUser.UserID, ViewModel.NewPassword)))
                {
                    return RedirectToAction("Manage");
                }
            }
            return View(ViewModel);
        }

        public ActionResult ApproveUser()
        {
            if( Common.Props.LoginUser == null || Common.Props.LoginUser.Role != eUserRoleID.Admin)
            {
                return RedirectToAction("PermissionDenied", "Home");
            }
            DAL.Users.UserDAL UserDALObj = new DAL.Users.UserDAL();
            return View(UserDALObj.GetPendingApprovalUserList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApproveUser(int UserID)
        {
            if(ModelState.IsValid)
            {
                DAL.Users.UserDAL UserDALObj = new DAL.Users.UserDAL();
                if(Common.Functions.SetAfterSaveResult(ModelState, UserDALObj.ApproveUser(UserID)))
                {
                    return RedirectToAction("ApproveUser");
                }
            }
            return View();
        }
    }
}