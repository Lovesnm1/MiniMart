//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DesignUI
{
    using System;
    using System.Collections.Generic;
    
    public partial class GoodsReceived
    {
        public string Code { get; set; }
        public string ID_Goods { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<double> Price { get; set; }
        public string Unit { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<double> Total { get; set; }
        public string Note { get; set; }
        public string Seller { get; set; }
        public string Address { get; set; }
        public Nullable<int> Phone { get; set; }
        public Nullable<bool> Hide { get; set; }
    
        public virtual Good Good { get; set; }
    }
}