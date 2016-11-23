using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UCAOrderManager.DAL.Customer;
using UCAOrderManager.Models.Customer;
using UCAOrderManager.Models.Template;

namespace UCAOrderManager.Controllers.Customer
{
    public class CustomerController : Controller
    {
        private CustomerDAL DALObj = new CustomerDAL();
 
        public ActionResult Index()
        {
            if (Common.Props.LoginUser == null)
            {
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/Customer/Index" });
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
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/Customer/Details/" + id.Value.ToString() });
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
            //if (Common.Props.LoginUser == null)
            //{
            //    return RedirectToAction("Login", "Users", new { ReturnUrl = "/Customer/Create" });
            //}
            //else if (Common.Props.LoginUser != null && Common.Props.LoginUser.Role != Models.Users.eUserRoleID.Admin)
            //{
            //    return RedirectToAction("PermissionDenied", "Home");
            //}

            //if (Common.Props.LoginUser == null || Common.Props.LoginUser.Role == Models.Users.eUserRoleID.Admin)
            //{
            //    return RedirectToAction("PermissionDenied", "Home");
            //}

            ViewBag.NewCustomerCode = 0;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,BusinessName,ContactName,EMailID,Password,Address,City,Postcode,Country,IntPhoneNo,AirportDestCity,IsApproved")] Models.Customer.CustomerViewModel ViewModel)
        {
            //if (Common.Props.LoginUser == null)
            //{
            //    return RedirectToAction("Login", "Users", new { ReturnUrl = "/Customer/Create" });
            //}
            //else if (Common.Props.LoginUser != null && Common.Props.LoginUser.Role != Models.Users.eUserRoleID.Admin)
            //{
            //    return RedirectToAction("PermissionDenied", "Home");
            //}

            if (ModelState.IsValid)
            {
                Models.Template.SavingResult SavingRes = DALObj.SaveRecord(ViewModel);
                if (Common.Functions.SetAfterSaveResult(ModelState, SavingRes))
                {
                    if (Common.Props.LoginUser == null)
                    {
                        //Task.Run( async() => {
                        //    await SendApprovalEmailToAdmins((int)SavingRes.PrimeKeyValue);
                        //});
                        SendApprovalEmailToAdmins((int)SavingRes.PrimeKeyValue);

                        return RedirectToAction("RegistrationConfirm", "Customer", new { UserID = (int)SavingRes.PrimeKeyValue });
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
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
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/Customer/Edit/" + id.Value.ToString() });
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
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/Customer/Delete/" + id.Value.ToString() });
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
                return RedirectToAction("Login", "Users", new { ReturnUrl = "/Customer/Delete/" + id.ToString() });
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
        public ActionResult IsEmailIDExists(string EmailID)
        {
            string v = (DALObj.CheckDuplicateEmailID(0, EmailID) ? "True" : "False");
            return Json(new { Response =  v });
        }

        public ActionResult RegistrationConfirm(int? UserID)
        {
            if (UserID == null)
            {
                return RedirectToAction("BadRequest", "Home");
            }

            CustomerViewModel ViewModel = DALObj.FindByID(UserID.Value);

            return View(ViewModel);
        }

        //public async Task SendApprovalEmailToAdmins(int UserID)
        public void SendApprovalEmailToAdmins(int UserID)
        {
            try
            {
                CustomerViewModel Customer = DALObj.FindByIDGetListViewModel(UserID);

                DAL.Users.UserDAL UserDALObj = new DAL.Users.UserDAL();
                List<Models.Users.UserAdminListViewModel> Admins = UserDALObj.GetAdminUsers();

                string Subject = "Activate " + Customer.BusinessName ?? "NoNames" + "'s account.";
                string MessageBody = string.Format(@"Hello,

A new customer recently registered on {0}
Here is the details.

Business Name : {1}
Contact Name : {2}
Address : {3}
City : {4}
Country : {5}

Please verify his/her account by clicking on the below link

{6}
If the above link is not working then please copy and paste it in your browser's address bar.
If this email is not relevant to you then please go to contact page on {0} and contact authority to stop sending emails to you.",
    Common.Props.CurrentDomainName,
    Customer.BusinessName,
    Customer.ContactName,
    Customer.Address ?? "",
    Customer.City ?? "" + " " + Customer.Postcode ?? "",
    Customer.Country,
    Common.Props.CurrentDomainName + @"/Customer/ApproveCustomer/" + UserID.ToString());

                string SendToIds = "";
                foreach (Models.Users.UserAdminListViewModel admin in Admins)
                {
                    SendToIds += (SendToIds != "" ? "," : "") + admin.EMailID;
                }

                Common.Functions.SendEmailFromNoReply(SendToIds, Subject, MessageBody);
                
                //Common.Functions.SendEmailFromNoReply(SendToIds, Subject, MessageBody);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
                RedirectToAction("Error");
            }
        }

        public ActionResult ApproveCustomer(int? id)
        {
            if(id == null)
            {
                return RedirectToAction("BadRequest", "Home");
            }
            CustomerViewModel ViewModel = DALObj.FindByID(id.Value);
            return View(ViewModel);
        }

        [HttpPost]
        public ActionResult ApproveCustomer(int id, bool Approved)
        {
            SavingResult SavingRes = DALObj.ApproveUser(id, Approved);

            CustomerViewModel ViewModel = DALObj.FindByID(id);

            if (Common.Functions.SetAfterSaveResult(ModelState, SavingRes))
            {
                CustomerViewModel Customer = DALObj.FindByID(id);

                string SendTo = null;
                string Subject = null;
                string Body = null;
                if (Approved)
                {
                    SendTo = Customer.EMailID;
                    Subject = "You account has been approved.";
                    Body = string.Format(@"Dear sir,
Your account on {0} has been approved. Now you can place order on {0}.

Please login by clicking on the link below. 

{1}

If the above link is not working then please copy and paste the above link in address bar. If you have any issue, please contact support on {0}
", Common.Props.CurrentDomainName,
    Common.Props.CurrentDomainName + @"/Users/Login");

                    Common.Functions.SendEmailFromNoReply(SendTo, Subject, Body);
                }
                else
                {
                    SendTo = Customer.EMailID;
                    Subject = "You account has been rejected by admin.";
                    Body = string.Format(@"Dear customer,
Your account on {0} has been disapproved and rejected by admin. Please contact support on {0} for more details.", Common.Props.CurrentDomainName);

                    Common.Functions.SendEmailFromNoReply(SendTo, Subject, Body);
                }

                return RedirectToAction("ApproveCustomerConfirm", new { id = id });
            }

            return View(ViewModel);
        }

        public ActionResult ApproveCustomerConfirm(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequest", "Home");
            }
            CustomerViewModel ViewModel = DALObj.FindByID(id.Value);
            return View(ViewModel);
        }


        public JsonResult GetCustomerDetails(int? id)
        {
            if (id == null)
            {
                return Json("Bad request");
            }

            if (Common.Props.LoginUser == null)
            {
                return Json("Not authenticated");
            }
            else if (Common.Props.LoginUser != null && Common.Props.LoginUser.Role != Models.Users.eUserRoleID.Admin)
            {
                return Json("Not authorized");
            }

            CustomerViewModel ViewModel = DALObj.FindByID(id.Value);
            var ret = Json(ViewModel);
            return ret;
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