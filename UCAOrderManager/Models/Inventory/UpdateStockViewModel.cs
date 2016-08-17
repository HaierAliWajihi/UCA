using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UCAOrderManager.Models.Inventory
{
    public class UpdateStockViewModel
    {
        [Browsable(false)]
        public int ProductID { get; set; }

        [DisplayName("Product Code")]
        public int ProductCode { get; set; }

        [DisplayName("Scientific Name")]
        public string ScientificName { get; set; }

        [DisplayName("Common Name")]
        public string CommonName { get; set; }

        [DisplayName("Description")]
        public string Descr { get; set; }

        [DisplayName("Size")]
        public string Size { get; set; }

        [DisplayName("Cultivation Type")]
        public string CultivationType { get; set; }

        public decimal Rate { get; set; }

        [DisplayName("Stock")]
        [DisplayFormat(DataFormatString = "0",
            ApplyFormatInEditMode = true,
            ConvertEmptyStringToNull = false,
            NullDisplayText = "0")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public int CurrentStock { get; set; }
    }
}