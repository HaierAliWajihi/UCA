﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class dbUltraCoralEntities : DbContext
    {
        public dbUltraCoralEntities()
            : base("name=dbUltraCoralEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tblCompany> tblCompanies { get; set; }
        public virtual DbSet<tblProductCommonName> tblProductCommonNames { get; set; }
        public virtual DbSet<tblProductCultivationType> tblProductCultivationTypes { get; set; }
        public virtual DbSet<tblProductOpeningStock> tblProductOpeningStocks { get; set; }
        public virtual DbSet<tblProductRate> tblProductRates { get; set; }
        public virtual DbSet<tblProductScientificName> tblProductScientificNames { get; set; }
        public virtual DbSet<tblSaleInvoiceProductDetail> tblSaleInvoiceProductDetails { get; set; }
        public virtual DbSet<tblSaleOrder> tblSaleOrders { get; set; }
        public virtual DbSet<tblSaleOrderProductDetail> tblSaleOrderProductDetails { get; set; }
        public virtual DbSet<tblUpdateInventoryLog> tblUpdateInventoryLogs { get; set; }
        public virtual DbSet<tblUserRole> tblUserRoles { get; set; }
        public virtual DbSet<tblUsersLoginLog> tblUsersLoginLogs { get; set; }
        public virtual DbSet<tblUser> tblUsers { get; set; }
        public virtual DbSet<tblSaleInvoice> tblSaleInvoices { get; set; }
        public virtual DbSet<tblSaleInvoiceStatu> tblSaleInvoiceStatus { get; set; }
        public virtual DbSet<tblProductSize> tblProductSizes { get; set; }
        public virtual DbSet<tblProduct> tblProducts { get; set; }
        public virtual DbSet<tblBoxList> tblBoxLists { get; set; }
        public virtual DbSet<tblBoxListBoxDetail> tblBoxListBoxDetails { get; set; }
        public virtual DbSet<tblBoxListProductDetail> tblBoxListProductDetails { get; set; }
    }
}
