using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UCAOrderManager.Models
{


    public class IsNumericAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                decimal val;
                var isNumeric = decimal.TryParse(value.ToString(), out val);

                if (!isNumeric)
                {
                    return new ValidationResult("Must be numeric");
                }
            }

            return ValidationResult.Success;
        }
    }
}