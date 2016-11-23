using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UCAOrderManager.Models.Product
{
    public class ProductSizeViewModel
    {
        [Browsable(false)]
        public int ProductSizeID { get; set; }

        [DisplayName("Size Name")]
        [StringLength(50, ErrorMessage = "{0} can be max {1} chars long.")]
        [Required]
        public string ProductSizeName { get; set; }

        [DisplayName("Quantity Required")]
        public bool QuanReq { get; set; }
    }

    public class ProductSizeSelectListViewModel
    {
        public int ProductSizeID { get; set; }

        [DisplayName("Size Name")]
        public string ProductSizeName { get; set; }
    }
}