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
    
    public partial class yillik_maas_raporlari
    {
        public int Id { get; set; }
        public Nullable<int> personel_id { get; set; }
        public Nullable<System.DateTime> yil { get; set; }
        public Nullable<double> toplam_prim_gunu_sayisi { get; set; }
        public Nullable<double> toplam_prim_saati { get; set; }
        public Nullable<double> toplam_brut_ucret { get; set; }
        public Nullable<double> toplam_brut_toplam { get; set; }
        public Nullable<double> toplam_fazla_mesai { get; set; }
        public Nullable<double> toplam_sair_odeme { get; set; }
        public Nullable<double> toplam_vergi_matrahi { get; set; }
        public Nullable<double> toplam_sigorta_matrahi { get; set; }
        public Nullable<double> toplam_damga_vergisi { get; set; }
        public Nullable<double> toplam_gelir_vergisi { get; set; }
        public Nullable<double> toplam_asgari_gecim_indirimi { get; set; }
        public Nullable<double> toplam_sigorta_kesintisi { get; set; }
        public Nullable<double> toplam_issizlik_sigortasi_kesintisi { get; set; }
        public Nullable<double> toplam_isveren_sgk_istisnasi { get; set; }
        public Nullable<double> toplam_net_ucret { get; set; }
        public Nullable<double> toplam_odenecek_ucret { get; set; }
        public Nullable<double> sigorta_primi_isveren { get; set; }
        public Nullable<double> issizlik_sigortasi_isveren { get; set; }
        public Nullable<double> toplam_sgk_primi { get; set; }
        public Nullable<double> odenecek_sgk_primi { get; set; }
        public Nullable<double> odenecek_gelir_vergisi { get; set; }
        public Nullable<double> odenecek_issizlik_sigorta_kesintisi { get; set; }
        public Nullable<double> toplam1 { get; set; }
        public Nullable<double> toplam2 { get; set; }
        public Nullable<double> net_odenecek_sgk_primi { get; set; }
    
        public virtual personel personel { get; set; }
    }
}
