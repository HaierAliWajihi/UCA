using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UCAOrderManager.Models.Product
{
    public class ProductCommonNameViewModel
    {
        [Browsable(false)]
        public int ProductCommonNameID { get; set; }

        [DisplayName("Common Name")]
        [StringLength(50, ErrorMessage = "{0} can be max {1} chars long.")]
        [Required]
        public string ProductCommonName { get; set; }
    }

    public class ProductCommonNameSelectListViewModel
    {
        public int ProductCommonNameID { get; set; }

        [DisplayName("Common Name")]
        public string ProductCommonName { get; set; }
    }
}