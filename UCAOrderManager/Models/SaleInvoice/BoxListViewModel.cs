using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UCAOrderManager.Models.SaleInvoice
{
    public class BoxListViewModel
    {
        [Browsable(false)]
        public int BoxListID { get; set; }
        
        [Browsable(false)]
        public int SaleInvoiceID { get; set; }

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

        [DisplayName("International Phone #")]
        [StringLength(50, ErrorMessage = "{0} can be max {1} chars long.")]
        public string IntPhoneNo { get; set; }

        [DisplayName("Email Contact")]
        [StringLength(50, ErrorMessage = "{0} can be max {1} chars long.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email id")]
        public string EMailContact { get; set; }

        [DisplayName("Airport Destination (City)")]
        [StringLength(50, ErrorMessage = "{0} can be max {1} chars long.")]
        public string AirportDestCity { get; set; }

        [DisplayName("Shipping Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM'/'dd'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ShippingDate { get; set; }

        [DisplayName("Arrival Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM'/'dd'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ArrivalDate { get; set; }

        [DisplayName("Domestic Flight")]
        [StringLength(50, ErrorMessage = "{0} can be max {1} chars long.")]
        public string DomesticFlight { get; set; }

        [DisplayName("International Flight")]
        [StringLength(50, ErrorMessage = "{0} can be max {1} chars long.")]
        public string InternationalFlight { get; set; }

        [Browsable(false)]
        [DisplayName("Products")]
        public List<BoxListBoxDetailViewModel> BoxListDetails { get; set; }

    }

    public class BoxListBoxDetailViewModel
    {
        [Browsable(false)]
        public int BoxListBoxDetailID { get; set; }

        [Browsable(false)]
        public int BoxID { get; set; }

        [DisplayName("Box#")]
        public int BoxNo { get; set; }

        [DisplayName("Total Qty")]
        public int TotalQuan { get; set; }

        public List<BoxListProductDetailViewModel> Products { get; set; }
    }

    public class BoxListProductDetailViewModel
    {
        public int BoxListProductDetailID { get; set; }

        public int BoxListBoxDetailID { get; set; }

        public int ScientificNameID { get; set; }

        public string ScientificName { get; set; }

        public int Quan { get; set; }
    }
}