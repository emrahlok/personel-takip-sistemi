//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Personel_Finansal_Takip.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class is_giris_cikis_tur
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public is_giris_cikis_tur()
        {
            this.is_giris_cikis = new HashSet<is_giris_cikis>();
        }
    
        public int Id { get; set; }
        public string tur { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<is_giris_cikis> is_giris_cikis { get; set; }
    }
}