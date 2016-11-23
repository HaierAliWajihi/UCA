using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UCAOrderManager.Models.Customer
{
    public class CustomerViewModel
    {
        [Browsable(false)]
        public int UserID { get; set; }

        [DisplayName("Business Name")]
        [StringLength(50, ErrorMessage = "{0} can be max {1} chars long.")]
        [Required]
        public string BusinessName { get; set; }

        [DisplayName("Contact Name")]
        [StringLength(50, ErrorMessage = "{0} can be max {1} chars long.")]
        [Required]
        public string ContactName { get; set; }

        [DisplayName("House/Building # + Street Address")]
        [StringLength(500, ErrorMessage = "{0} can be max {1} chars long.")]
        public string Address { get; set; }

        [DisplayName("City/Location")]
        [StringLength(50, ErrorMessage = "{0} can be max {1} chars long.")]
        public string City { get; set; }

        [DisplayName("Post/Zip code")]
        [StringLength(10, ErrorMessage = "{0} can be max {1} chars long.")]
        public string Postcode { get; set; }

        [DisplayName("Country")]
        [StringLength(50, ErrorMessage = "{0} can be max {1} chars long.")]
        public string Country { get; set; }

        [DisplayName("Email"), DataType( System.ComponentModel.DataAnnotations.DataType.EmailAddress)]
        [StringLength(50, ErrorMessage = "{0} can be max {1} chars long.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email id")]
        public string EMailID { get; set; }

        [DisplayName("Password")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("International Phone #")]
        [StringLength(50, ErrorMessage = "{0} can be max {1} chars long.")]
        public string IntPhoneNo { get; set; }

        [DisplayName("Airport Destination (City)")]
        [StringLength(50, ErrorMessage = "{0} can be max {1} chars long.")]
        public string AirportDestCity { get; set; }

        [DisplayName("Approved")]
        public bool? IsApproved { get; set; }
    }

    public class CustomerListViewModel
    {
        [Browsable(false)]
        public int UserID { get; set; }

        [DisplayName("Business Name")]
        public string BusinessName { get; set; }

        [DisplayName("Contact Name")]
        public string ContactName { get; set; }

        [DisplayName("E-Mail")]
        public string EMailID { get; set; }

        [DisplayName("House/Building # + Street Address")]
        public string Address { get; set; }

        [DisplayName("City/Location")]
        public string City { get; set; }

        [DisplayName("Post/Zip code")]
        public string Postcode { get; set; }

        [DisplayName("Country")]
        public string Country { get; set; }

        [DisplayName("Approved")]
        public bool? IsApproved { get; set; }
    }
}