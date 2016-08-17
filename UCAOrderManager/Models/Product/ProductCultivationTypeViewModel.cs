using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UCAOrderManager.Models.Product
{
    public class ProductCultivationTypeViewModel
    {
        [Browsable(false)]
        public int ProductCultivationTypeID { get; set; }

        [DisplayName("Cultivation Type")]
        [StringLength(50, ErrorMessage = "{0} can be max {1} chars long.")]
        [Required]
        public string ProductCultivationTypeName { get; set; }

    }

    public class ProductCultivationTypeSelectListViewModel
    {
        public int ProductCultivationTypeID { get; set; }

        [DisplayName("Cultivation Type")]
        public string ProductCultivationTypeName { get; set; }
    }
}