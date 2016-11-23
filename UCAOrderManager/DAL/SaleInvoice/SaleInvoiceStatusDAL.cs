using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCAOrderManager.Models.SaleInvoice;

namespace UCAOrderManager.DAL.SaleInvoice
{
    public class SaleInvoiceStatusDAL
    {
        public static SelectList GetSelectionList()
        {
            using (dbUltraCoralEntities db = new dbUltraCoralEntities())
            {
                var list = (from r in db.tblSaleInvoiceStatus
                            select new SaleInvoiceStatusSelectListModel()
                            {
                                SaleInvoiceStatusID = r.SaleInvoiceStatusID,
                                SaleInvoiceStatusName = r.SaleInvoiceStatusName
                            }).ToList();

                return new SelectList(list, "SaleInvoiceStatusID", "SaleInvoiceStatusName");
            }
        }
    }
}