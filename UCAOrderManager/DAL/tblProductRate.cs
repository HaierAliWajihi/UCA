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
    
    public partial class tblProductRate
    {
        public int ProductRateID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public System.DateTime WED { get; set; }
        public decimal Rate { get; set; }
        public Nullable<System.DateTime> rcdt { get; set; }
        public Nullable<int> rcuid { get; set; }
        public Nullable<System.DateTime> redt { get; set; }
        public Nullable<int> reuid { get; set; }
    
        public virtual tblUser tblUser { get; set; }
        public virtual tblUser tblUser1 { get; set; }
        public virtual tblProduct tblProduct { get; set; }
    }
}