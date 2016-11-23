using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace UCAOrderManager.Models.SaleInvoice
{
    public class SaleInvoiceStatusSelectListModel
    {
        [Browsable(false)]
        public int SaleInvoiceStatusID { get; set; }

        [DisplayName("Status")]
        public string SaleInvoiceStatusName { get; set; }
    }
}