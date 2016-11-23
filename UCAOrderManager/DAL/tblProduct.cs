//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UCAOrderManager.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblProduct
    {
        public tblProduct()
        {
            this.tblUpdateInventoryLogs = new HashSet<tblUpdateInventoryLog>();
            this.tblProductOpeningStocks = new HashSet<tblProductOpeningStock>();
            this.tblProductRates = new HashSet<tblProductRate>();
            this.tblSaleInvoiceProductDetails = new HashSet<tblSaleInvoiceProductDetail>();
            this.tblSaleOrderProductDetails = new HashSet<tblSaleOrderProductDetail>();
        }
    
        public int ProductID { get; set; }
        public int ProductCode { get; set; }
        public int ScientificNameID { get; set; }
        public int CommonNameID { get; set; }
        public string Descr { get; set; }
        public int SizeID { get; set; }
        public int CultivationTypeID { get; set; }
        public decimal Rate { get; set; }
        public decimal CurrentStock { get; set; }
        public decimal RateUpliftPerc { get; set; }
        public decimal UpliftedRate { get; set; }
        public Nullable<System.DateTime> rcdt { get; set; }
        public Nullable<int> rcuid { get; set; }
        public Nullable<System.DateTime> redt { get; set; }
        public Nullable<int> reuid { get; set; }
    
        public virtual ICollection<tblUpdateInventoryLog> tblUpdateInventoryLogs { get; set; }
        public virtual tblUser tblUser { get; set; }
        public virtual tblUser tblUser1 { get; set; }
        public virtual tblProductCommonName tblProductCommonName { get; set; }
        public virtual tblProductCultivationType tblProductCultivationType { get; set; }
        public virtual tblProductScientificName tblProductScientificName { get; set; }
        public virtual tblProductSize tblProductSize { get; set; }
        public virtual ICollection<tblProductOpeningStock> tblProductOpeningStocks { get; set; }
        public virtual ICollection<tblProductRate> tblProductRates { get; set; }
        public virtual ICollection<tblSaleInvoiceProductDetail> tblSaleInvoiceProductDetails { get; set; }
        public virtual ICollection<tblSaleOrderProductDetail> tblSaleOrderProductDetails { get; set; }
    }
}
