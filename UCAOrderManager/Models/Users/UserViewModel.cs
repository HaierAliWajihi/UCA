using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UCAOrderManager.Models.Users
{
    public class UserRegistrationViewModel
    {
        [Browsable(false)]
        public int UserID { get; set; }

        [DisplayName("Full Name")]
        [MaxLength(50)]
        [Required(ErrorMessage="Please enter full name")]
        public string FullName { get; set; }

        [DisplayName("EMail ID")]
        [MaxLength(50)]
        [Required(ErrorMessage="Please enter a valid email id")]
        [EmailAddress(ErrorMessage = "Please enter a valid email id")]
        public string EMailID { get; set; }

        [DisplayName("Password")]
        [MaxLength(50)]
        [Required(ErrorMessage="Please enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class UserLoginDetails
    {
        [Browsable(false)]
        public int UserID { get; set; }

        [DisplayName("Full Name")]
        public string FullName { get; set; }

        [DisplayName("Email id")]
        public string EMailID { get; set; }

        [Browsable(false)]
        public eUserRoleID Role { get; set; }

        [Browsable(false)]
        public bool IsApproved { get; set; }
    }

    public enum eUserRoleID
    {
        Admin = 1,
        User = 2
    }

    public class UserLoginViewModel
    {
        [DisplayName("Email id")]
        [EmailAddress()]
        [Required]
        public string EmailID { get; set; }

        [Required]
        [DisplayName("Password")]
        [DataType( System.ComponentModel.DataAnnotations.DataType.Password)]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        public string Password { get; set; }

        [DisplayName("Remember me")]
        public bool RememberMe { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class UserApprovalViewModel
    {
        [Browsable(false)]
        public int UserID { get; set; }

        [DisplayName("Full Name")]
        public string FullName { get; set; }

        [DisplayName("Email id")]
        public string EMailID { get; set; }
    }
}