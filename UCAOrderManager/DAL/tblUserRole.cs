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
    
    public partial class tblUserRole
    {
        public tblUserRole()
        {
            this.tblUsers = new HashSet<tblUser>();
        }
    
        public int UserRoleID { get; set; }
        public string UserRoleName { get; set; }
    
        public virtual ICollection<tblUser> tblUsers { get; set; }
    }
}
