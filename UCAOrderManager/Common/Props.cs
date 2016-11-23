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

        //static Models.Company.CompanyViewModel CompanyProfile_;
        public static Models.Company.CompanyViewModel CompanyProfile
        {
            get
            {
                var SesVar = HttpContext.Current.Session["CompanyProfile"];

                if (SesVar == null)
                {
                    DAL.Company.CompanyDAL CompanyDALObj = new DAL.Company.CompanyDAL();
                    Models.Company.CompanyViewModel temp = CompanyDALObj.GetFirstCompanyDetail();
                    SesVar = temp;
                    return temp;
                }
                return (Models.Company.CompanyViewModel)SesVar;
            }
            set
            {
                HttpContext.Current.Session["CompanyProfile"] = value;
            }
        }

        public static string CurrentDomainName
        {
            get
            {
                return HttpContext.Current.Request.Url.Scheme +
                    System.Uri.SchemeDelimiter + HttpContext.Current.Request.Url.Host +
                    (HttpContext.Current.Request.Url.IsDefaultPort ? "" : ":" +
                    HttpContext.Current.Request.Url.Port);
            }
        }
    }
}