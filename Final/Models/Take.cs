//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Final.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Take
    {
        public int take_id { get; set; }
        public int s_id { get; set; }
        public int test_id { get; set; }
        public int marks { get; set; }
    
        public virtual Student Student { get; set; }
        public virtual Test Test { get; set; }
    }
}
