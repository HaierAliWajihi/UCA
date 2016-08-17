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
        public int CustomerID { get; set; }

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

        [DisplayName("City/Location +Postal/Zip Code")]
        [StringLength(50, ErrorMessage = "{0} can be max {1} chars long.")]
        public string City { get; set; }

        [DisplayName("Country")]
        [StringLength(50, ErrorMessage = "{0} can be max {1} chars long.")]
        public string Country { get; set; }

        [DisplayName("Email"), DataType( System.ComponentModel.DataAnnotations.DataType.EmailAddress)]
        [StringLength(50, ErrorMessage = "{0} can be max {1} chars long.")]
        public string EMailID { get; set; }

        [DisplayName("International Phone #")]
        [StringLength(50, ErrorMessage = "{0} can be max {1} chars long.")]
        public string IntPhoneNo { get; set; }

        [DisplayName("Airport Destination (City)")]
        [StringLength(50, ErrorMessage = "{0} can be max {1} chars long.")]
        public string AirportDestCity { get; set; }

        [DisplayName("Approved")]
        public bool IsApproved { get; set; }
    }

    public class CustomerListViewModel
    {
        [Browsable(false)]
        public int CustomerID { get; set; }

        [DisplayName("Business Name")]
        public string BusinessName { get; set; }

        [DisplayName("Contact Name")]
        public string ContactName { get; set; }

        [DisplayName("Address")]
        public string Address { get; set; }

        [DisplayName("City")]
        public string City { get; set; }

        [DisplayName("Country")]
        public string Country { get; set; }

        [DisplayName("E-Mail")]
        public string EMailID { get; set; }

        [DisplayName("Approved")]
        public bool IsApproved { get; set; }
    }

}