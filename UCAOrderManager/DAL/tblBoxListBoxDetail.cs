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
    
    public partial class tblBoxListBoxDetail
    {
        public tblBoxListBoxDetail()
        {
            this.tblBoxListProductDetails = new HashSet<tblBoxListProductDetail>();
        }
    
        public int BoxListBoxDetailID { get; set; }
        public int BoxListID { get; set; }
        public int BoxNo { get; set; }
        public int TotalQuan { get; set; }
    
        public virtual tblBoxList tblBoxList { get; set; }
        public virtual ICollection<tblBoxListProductDetail> tblBoxListProductDetails { get; set; }
    }
}
