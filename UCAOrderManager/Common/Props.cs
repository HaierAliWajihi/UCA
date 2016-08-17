using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace UCAOrderManager.Common
{
    public static class Props
    {
        public static Models.Users.UserLoginDetails LoginUser
        {
            get
            {
                var SesVar = HttpContext.Current.Session["LoginUser"];
                if(SesVar == null)
                {
                    HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                    if (authCookie == null) return null;
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);

                    if (ticket == null) return null;

                    JavaScriptSerializer js = new JavaScriptSerializer();
                    var user = js.Deserialize(ticket.UserData, typeof(Models.Users.UserLoginDetails));
                    if(user == null) return null; 

                    Models.Users.UserLoginDetails myuser = (Models.Users.UserLoginDetails)user;
                    LoginUser = myuser;
                    return myuser;
                    
                }
                return (Models.Users.UserLoginDetails)SesVar;
            }
            set
            {
                HttpContext.Current.Session["LoginUser"] = value;
            }
        }

        static Models.Company.CompanyViewModel CompanyProfile_;
        public static Models.Company.CompanyViewModel CompanyProfile
        {
            get
            {
                if(CompanyProfile_ == null)
                {
                    DAL.Company.CompanyDAL CompanyDALObj = new DAL.Company.CompanyDAL();
                    CompanyProfile_ = CompanyDALObj.GetFirstCompanyDetail();
                }
                return CompanyProfile_;
            }
            set
            {
                CompanyProfile_ = value;
            }
        }
    }
}