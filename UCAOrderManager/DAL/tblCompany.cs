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
    
    public partial class tblCompany
    {
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string FaxNo { get; set; }
        public string EMailID { get; set; }
        public string Website { get; set; }
        public string Slogan { get; set; }
        public byte[] Logo { get; set; }
        public Nullable<System.DateTime> rcdt { get; set; }
        public Nullable<int> rcuid { get; set; }
        public Nullable<System.DateTime> redt { get; set; }
        public Nullable<int> reuid { get; set; }
    
        public virtual tblUser tblUser { get; set; }
        public virtual tblUser tblUser1 { get; set; }
    }
}