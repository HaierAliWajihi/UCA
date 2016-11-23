using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UCAReports.SaleInvoice
{
    public class SaleInvoiceReportDataSet
    {
        public List<SaleInvoiceReportModel> SaleInvoiceHeader { get; set; }

        public List<SaleInvoiceProducDetailReportModel> SaleInvoiceProductDetail { get; set; }
    }

    public class SaleInvoiceReportModel
    {
        [DisplayName("Sale Invoice ID")]
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
        public int? SaleOrderID { get; set; }

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

        public string AddressLine1
        {
            get
            {
                string Add = "";

                if(!String.IsNullOrWhiteSpace(Address))
                {
                    Add += (!String.IsNullOrWhiteSpace(Add) ? ", " : "") + Address;
                }
                return Add;
            }
        }

        public string AddressLine2
        {
            get
            {
                string Add = "";

                if (!String.IsNullOrWhiteSpace(City))
                {
                    Add += (!String.IsNullOrWhiteSpace(Add) ? ", " : "") + City;
                }
                if (!String.IsNullOrWhiteSpace(Country))
                {
                    Add += (!String.IsNullOrWhiteSpace(Add) ? ", " : "") + Country;
                }
                if (!String.IsNullOrWhiteSpace(Postcode))
                {
                    Add += (!String.IsNullOrWhiteSpace(Add) ? ", " : "") + Postcode;
                }
                return Add;
            }
        }

        [DisplayName("Email Contact")]
        public string EmailContact { get; set; }

        [DisplayName("Airport Destination (City)")]
        [StringLength(50, ErrorMessage = "{0} can be max {1} chars long.")]
        public string AirportDestCity { get; set; }

        [DisplayName("Est. Del. Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:MM'/'dd'/'yyyy}")]
        public DateTime? EstDelDate { get; set; }

        [DisplayName("AWB#")]
        [StringLength(50, ErrorMessage = "{0} can be max {1} chars long.")]
        public string AWBNo { get; set; }

        [DisplayName("Flight 1")]
        [StringLength(50, ErrorMessage = "{0} can be max {1} chars long.")]
        public string Flight1 { get; set; }

        [DisplayName("Flight 2")]
        [StringLength(50, ErrorMessage = "{0} can be max {1} chars long.")]
        public string Flight2 { get; set; }

        [DisplayName("Total Qty")]
        public decimal TotalQuan { get; set; }

        [DisplayName("Total G.Amt")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        public decimal TotalGAmt { get; set; }

        [DisplayName("Domestic Freight Charges")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        public decimal DomesticFreightCharges { get; set; }

        [DisplayName("Box Charges")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        public decimal BoxCharges { get; set; }

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

    public class SaleInvoiceProducDetailReportModel
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
    }

    public class SaveInvoiceFooter
    {
        public int SaleInvoiceID { get; set; }
    }
}