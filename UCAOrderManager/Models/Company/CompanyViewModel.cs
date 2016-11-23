using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;

namespace UCAOrderManager.Models.Company
{
    public class CompanyViewModel
    {
        public int CompanyID { get; set; }

        [Required]
        [DisplayName("Company Name")]
        [StringLength(50, ErrorMessage = "The {0} can be {1} characters long.")]
        public string CompanyName { get; set; }

        [DisplayName("Address")]
        [StringLength(1000, ErrorMessage = "The {0} can be {1} characters long.")]
        public string Address { get; set; }

        [DisplayName("Phone No")]
        [StringLength(50, ErrorMessage = "The {0} can be {1} characters long.")]
        public string PhoneNo { get; set; }

        [DisplayName("Fax")]
        [StringLength(50, ErrorMessage = "The {0} can be {1} characters long.")]
        public string FaxNo { get; set; }

        [DisplayName("Email ID")]
        [EmailAddress]
        [StringLength(50, ErrorMessage = "The {0} can be {1} characters long.")]
        public string EMailID { get; set; }

        [DisplayName("Website")]
        [Url]
        [StringLength(50, ErrorMessage = "The {0} can be {1} characters long.")]
        public string Website { get; set; }

        [DisplayName("Slogan")]
        [StringLength(200, ErrorMessage = "The {0} can be {1} characters long.")]
        public string Slogan { get; set; }

        [DisplayName("Logo")]
        public Image Logo { get; set; }

        [Browsable(false)]
        public string noreplyEmailID { get; set; }

        [Browsable(false)]
        public string noreplyPassword { get; set; }

        [Browsable(false)]
        public string noreplyOutgoingSMTPServerName { get; set; }

        [Browsable(false)]
        public int noreplyOutgoingSMTPPortNo { get; set; }
    }
}