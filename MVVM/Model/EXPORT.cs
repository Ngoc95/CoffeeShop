//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuanLiCoffeeShop.MVVM.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class EXPORT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EXPORT()
        {
            this.EXPORT_INFO = new HashSet<EXPORT_INFO>();
        }
    
        public int EXP_ID { get; set; }
        public Nullable<int> EMP_ID { get; set; }
        public Nullable<System.DateTime> EXP_DATE { get; set; }
        public Nullable<decimal> TOTAL_COST { get; set; }
        public Nullable<bool> IS_DELETED { get; set; }
    
        public virtual EMPLOYEE EMPLOYEE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EXPORT_INFO> EXPORT_INFO { get; set; }
    }
}
