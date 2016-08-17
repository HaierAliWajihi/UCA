using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
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

    }
}