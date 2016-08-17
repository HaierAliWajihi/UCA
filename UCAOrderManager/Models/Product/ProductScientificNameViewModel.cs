using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UCAOrderManager.Models.Product
{
    public class ProductScientificNameViewModel
    {
        [Browsable(false)]
        public int ProductScientificNameID { get; set; }

        [DisplayName("Scientific Name")]
        [StringLength(50, ErrorMessage="{0} can be max {1} chars long.")]
        [Required]
        public string ProductScientificName { get; set; }

        [DisplayName("Is Alive")]
        [UIHint("Is it currently is in use ?")]
        public bool IsAlive { get; set; }
    }

    public class ProductScientificNameSelectListViewModel
    {
        public int ProductScientificNameID { get; set; }

        [DisplayName("Scientific Name")]
        public string ProductScientificName { get; set; }
    }
}