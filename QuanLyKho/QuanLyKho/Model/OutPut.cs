//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuanLyKho.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class OutPut
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OutPut()
        {
            this.OutPutInfoes = new HashSet<OutPutInfo>();
        }
    
        public string Id { get; set; }
        public Nullable<System.DateTime> DateOutput { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OutPutInfo> OutPutInfoes { get; set; }
    }
}
