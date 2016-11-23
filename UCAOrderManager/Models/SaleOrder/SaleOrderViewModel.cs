using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UCAOrderManager.Models.SaleOrder
{
    public class SaleOrderViewModel
    {
        [Browsable(false)]
        public int SaleOrderID { get; set; }

        [Required]
        [DisplayName("SO Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM'/'dd'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime SODate { get; set; }

        [DisplayName("SO #")]
        [ReadOnly(true)]
        public int SONo { get; set; }

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

        [DisplayName("Est. Del. Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:MM'/'dd'/'yyyy}")]
        public DateTime? EstDelDate { get; set; }

        [Browsable(false)]
        [DisplayName("Products")]
        public List<SaleOrderProducDetailViewModel> Products { get; set; }
    }

    public class SaleOrderListViewModel
    {
        [Browsable(false)]
        public int SaleOrderID { get; set; }

        [Browsable(false)]
        public int? SaleInvoiceID { get; set; }

        [DisplayName("SO Date")]
        [DisplayFormat(DataFormatString = "{0:MM'/'dd'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime SODate { get; set; }

        [DisplayName("SO #")]
        public int SONo { get; set; }

        [Browsable(false)]
        public int CustomerID { get; set; }

        [DisplayName("Business Name")]
        [StringLength(50, ErrorMessage = "{0} can be max {1} chars long.")]
        [Required]
        public string BusinessName { get; set; }

        [DisplayName("City/Location +Postal/Zip Code")]
        [StringLength(50, ErrorMessage = "{0} can be max {1} chars long.")]
        public string City { get; set; }

        [DisplayName("Country")]
        [StringLength(50, ErrorMessage = "{0} can be max {1} chars long.")]
        public string Country { get; set; }

        [DisplayName("Order Qty")]
        public decimal TotalQty { get; set; }

        [DisplayName("Order Amt")]
        public decimal TotalAmt { get; set; }
    }

    public class SaleOrderSelectListViewModel
    {
        [Browsable(false)]
        public int SaleOrderID { get; set; }

        [DisplayName("SO Date")]
        public DateTime SODate { get; set; }

        [DisplayName("SO #")]
        public int SONo { get; set; }
    }

    public class SaleOrderProducDetailViewModel
    {
        [Browsable(false)]
        public int ProductID { get; set; }

        [DisplayName("Scientific Name")]
        public string ScientificName { get; set; }

        [DisplayName("Common Name")]
        public string CommonName { get; set; }

        [DisplayName("Description")]
        public string Descr { get; set; }

        [DisplayName("Size")]
        public string SizeName { get; set; }

        [DisplayName("Cultivation Type")]
        public string CultivationTypeName { get; set; }

        [DisplayName("Price ($USD)")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        public decimal Rate { get; set; }

        [DisplayName("Stock")]
        public decimal CurrentStock { get; set; }

        [DisplayName("Order Qty")]
        public decimal OrderQty { get; set; }
    }
}