//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SRLog.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblCustAddress
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public Nullable<bool> IsPrimaryAddress { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public string SiteName { get; set; }
        public string ProjectManager { get; set; }
    
        public virtual tblCustomer tblCustomer { get; set; }
    }
}