using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UCAOrderManager.Models.Template;

namespace UCAOrderManager.Common
{
    public static class Functions
    {
        public static bool SetAfterSaveResult(ModelStateDictionary ModelState, SavingResult res)
        {
            switch(res.ExecutionResult)
            {
                case eExecutionResult.CommitedSucessfuly:
                    return true;

                case eExecutionResult.ErrorWhileExecuting:
                    ModelState.AddModelError("", res.Exception.Message);
                    break;
                case eExecutionResult.ValidationError:
                    ModelState.AddModelError("", res.ValidationError);
                    break;
            }

            return false;
        }

        public static Exception FindFinalError(Exception ex)
        {
            if (ex.GetType() == typeof(DbEntityValidationException))
            {
                string err = "";
                DbEntityValidationException dbVErr = (DbEntityValidationException)ex;
                foreach (var x in dbVErr.EntityValidationErrors)
                {
                    foreach (var e in x.ValidationErrors)
                    {
                        err = err + "\r\n" + e.PropertyName + " = " + e.ErrorMessage + ".";
                    }
                }
                ex = new Exception(err);
            }
            else
            {
                while (ex.Message == "An error occurred while updating the entries. See the inner exception for details.")
                {
                    ex = ex.InnerException;
                }
            }
            return ex;
        }

        public static void SendEmailFromNoReply(string SendToIds, string Subject, string MessageBody)
        {
            // Command line argument must the the SMTP host.
            SmtpClient SMTPClient = new SmtpClient();
            SMTPClient.Host = Common.Props.CompanyProfile.noreplyOutgoingSMTPServerName;
            SMTPClient.Port = Common.Props.CompanyProfile.noreplyOutgoingSMTPPortNo;
            SMTPClient.EnableSsl = false;
            //SMTPClient.Timeout = 10000;
            SMTPClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            SMTPClient.UseDefaultCredentials = false;
            SMTPClient.Credentials = new System.Net.NetworkCredential(Common.Props.CompanyProfile.noreplyEmailID, Common.Props.CompanyProfile.noreplyPassword);

            //MailMessage mm = new MailMessage("donotreply@domain.com", "sendtomyemail@domain.co.uk", "test", "test");
            //mm.BodyEncoding = UTF8Encoding.UTF8;
            //mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;


            MailMessage Mail = new MailMessage(Common.Props.CompanyProfile.noreplyEmailID, SendToIds, Subject, MessageBody);
            SMTPClient.Send(Mail);
        }
    }
}