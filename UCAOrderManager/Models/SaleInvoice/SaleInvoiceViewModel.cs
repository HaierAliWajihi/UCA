using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UCAOrderManager.Models.SaleInvoice
{
    public class SaleInvoiceViewModel
    {
        [Browsable(false)]
        public int SaleInvoiceID { get; set; }

        [Required]
        [DisplayName("Invoice Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM'/'dd'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime InvoiceDate { get; set; }

        [DisplayName("Invoice #")]
        [Required]
        public int InvoiceNo { get; set; }

        [Browsable(false)]
        [Required(ErrorMessage = "Please select status.")]
        public int SaleInvoiceStatusID { get; set; }

        [DisplayName("Status")]
        public string SaleInvoiceStatus { get; set; }

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
        public List<SaleInvoiceProducDetailViewModel> Products { get; set; }

        [DisplayName("Total Qty")]
        public decimal TotalQuan { get; set; }

        [DisplayName("Total G.Amt")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        public decimal TotalGAmt { get; set; }

        [DisplayName("Est. Boxes")]
        public int EstBoxes { get; set; }

        [DisplayName("Box Charges")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        public decimal BoxCharges { get; set; }

        [DisplayName("Domestic Freight Charges")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        public decimal DomesticFreightCharges { get; set; }

        [DisplayName("Int. Freight Charges")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        public decimal IntFreightCharges { get; set; }

        [DisplayName("TT Fee")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        public decimal TTFee { get; set; }

        [DisplayName("Total Freight")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        public decimal TotalFreight { get; set; }

        [DisplayName("Previous Credit")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        public decimal PreviousCredit { get; set; }

        [DisplayName("Total Payable Amt")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        public decimal TotalPayableAmt { get; set; }
    }

    public class SaleInvoiceListViewModel
    {
        [Browsable(false)]
        public int SaleInvoiceID { get; set; }

        [DisplayName("Date")]
        [DisplayFormat(DataFormatString = "{0:MM'/'dd'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime InvoiceDate { get; set; }

        [DisplayName("Invoice #")]
        public int InvoiceNo { get; set; }

        [Browsable(false)]
        public int SaleInvoiceStatusID { get; set; }

        [DisplayName("Status")]
        public string SaleInvoiceStatus { get; set; }

        [Browsable(false)]
        public int CustomerID { get; set; }

        [DisplayName("Business Name")]
        [StringLength(50, ErrorMessage = "{0} can be max {1} chars long.")]
        [Required]
        public string BusinessName { get; set; }

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

        [DisplayName("Total Pieces")]
        public decimal TotalQty { get; set; }

        [Browsable(false)]
        public bool IsQtyReq { get; set; }

        [DisplayName("Est. Boxes")]
        public int EstBoxes { get; set; }
    }

    public class SaleInvoiceSelectListViewModel
    {
        [Browsable(false)]
        public int SaleInvoiceID { get; set; }

        [DisplayName("Date")]
        public DateTime InvoiceDate { get; set; }

        [DisplayName("Invoice #")]
        public int InvoiceNo { get; set; }
    }

    public class SaleInvoiceProducDetailViewModel
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

        //[DisplayName("Cultivation Type")]
        //public string CultivationTypeName { get; set; }

        [DisplayName("Price ($USD)")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        public decimal Rate { get; set; }

        [DisplayName("Qty")]
        public decimal Qty { get; set; }

        [DisplayName("Amt")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        public decimal Amt { get; set; }

        [Browsable(false)]
        public bool IsQtyReq { get; set; }
    }
}