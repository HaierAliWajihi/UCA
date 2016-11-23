using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UCAOrderManager.Models.Product
{
    public class ProductMasterViewModel
    {
        [Browsable(false)]
        public int ProductID { get; set; }

        [DisplayName("Product Code")]
        [Required]
        public int ProductCode { get; set; }

        [DisplayName("Scientific Name")]
        [Required]
        public int ScientificNameID { get; set; }

        [DisplayName("Common Name")]
        [Required]
        public int CommonNameID { get; set; }

        [DisplayName("Description")]
        [StringLength(500, ErrorMessage="{0} can be max {1} chars long.")]
        [Required(AllowEmptyStrings=true)]
        public string Descr { get; set; }

        [DisplayName("Size")]
        [Required]
        public int SizeID { get; set; }

        [DisplayName("Cultivation Type")]
        [Required]
        public int CultivationTypeID { get; set; }

        [DisplayFormat(DataFormatString="#0.00", 
            ApplyFormatInEditMode=true, 
            ConvertEmptyStringToNull=false, 
            NullDisplayText="0.00")]
        [DataType( System.ComponentModel.DataAnnotations.DataType.Currency)]
        public decimal Rate { get; set; }

        [DisplayName("Price Uplift %")]
        [DisplayFormat(DataFormatString = "%000.00",
            ApplyFormatInEditMode = true,
            ConvertEmptyStringToNull = false,
            NullDisplayText = "0")]
        public decimal RateUplift { get; set; }

        [DisplayName("Stock")]
        [DisplayFormat(DataFormatString = "#0.00",
            ApplyFormatInEditMode = true,
            ConvertEmptyStringToNull = false,
            NullDisplayText = "0.00")]
        [DataType( System.ComponentModel.DataAnnotations.DataType.Currency)]
        public decimal CurrentStock { get; set; }
    }

    public class ProductMasterListViewModel
    {
        [Browsable(false)]
        public int ProductID { get; set; }

        [DisplayName("Code")]
        public int ProductCode { get; set; }

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

        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        public decimal Rate { get; set; }

        [DisplayName("Price Uplift %")]
        public decimal RateUplift { get; set; }
        
        [DisplayName("Stock")]
        public decimal CurrentStock { get; set; }

        [Browsable(false)]
        public bool IsQtyReq { get; set; }
    }
}