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
    
    public partial class tblBoxListProductDetail
    {
        public int BoxListProductDetailID { get; set; }
        public int BoxListID { get; set; }
        public Nullable<int> BoxListBoxDetailID { get; set; }
        public int ProductScientificNameID { get; set; }
        public int Quan { get; set; }
    
        public virtual tblBoxList tblBoxList { get; set; }
        public virtual tblBoxListBoxDetail tblBoxListBoxDetail { get; set; }
        public virtual tblProductScientificName tblProductScientificName { get; set; }
    }
}
