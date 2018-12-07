using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Personel_Finansal_Takip.Models;

namespace Personel_Finansal_Takip.Areas.admin.Models
{
    public class IzinStatePartialView
    {
        personeltakipsistemiEntities db = new personeltakipsistemiEntities();
        public List<States> izinStates { get; set; }
        public string personel_title { get; set; }
        public IzinStatePartialView(int? personel_id)
        {
            var personel = db.personels.Find(personel_id);
            personel_title = personel.ad + " " + personel.soyad;
            int? yillikIzinCount = 0;
            foreach (var count in personel.izinlers.Where(x => x.izin_tur.tur.Equals("Yıllık izin") && x.izin_baslangic.Value.Year == DateTime.Now.Year && x.izin_bitis.Value.Year == DateTime.Now.Year && x.onay_tarihi != null).ToList())
                yillikIzinCount += count.sure.HasValue ? count.sure : 0;
            var yillikIzinStates = new States();
            yillikIzinStates.stateTitle = "Yıllık izin (";
            if(personel.ise_giris_tarihi.Value.AddYears(10).Date <= DateTime.Now.Date)
            {
                yillikIzinStates.remain = 30 - yillikIzinCount;
                yillikIzinStates.stateTitle += yillikIzinCount + "/30)";
                yillikIzinStates.percentage = (yillikIzinCount * 100) / 30;
                yillikIzinStates.statePercentage = yillikIzinStates.percentage + "%";
            }
            else if (personel.ise_giris_tarihi.Value.AddYears(1).Date <= DateTime.Now.Date)
            {
                yillikIzinStates.remain = 20 - yillikIzinCount;
                yillikIzinStates.stateTitle += yillikIzinCount + "/20)";
                yillikIzinStates.percentage = (yillikIzinCount * 100) / 20;
                yillikIzinStates.statePercentage = yillikIzinStates.percentage + "%";
            }
            else
            {
                yillikIzinStates.remain = 0;
                yillikIzinStates.stateTitle += "0/0)";
                yillikIzinStates.percentage = 100;
                yillikIzinStates.statePercentage = "100%";
            }
            int? raporluGunCount = 0;
            foreach (var count in personel.izinlers.Where(x => x.izin_tur.tur.Equals("Sıhhi (hastalık) raporu") && x.izin_baslangic.Value.Year == DateTime.Now.Year && x.izin_bitis.Value.Year == DateTime.Now.Year && x.onay_tarihi != null).ToList())
                raporluGunCount += count.sure.HasValue ? count.sure : 0;
            var raporluGunStates = new States();
            raporluGunStates.remain = 180 - raporluGunCount;
            raporluGunStates.stateTitle = "Sıhhi (hastalık) raporu (" + raporluGunCount + "/180)";
            raporluGunStates.percentage = (raporluGunCount * 100) / 180;
            raporluGunStates.statePercentage = raporluGunStates.percentage + "%";
            int? ucretliIzinCount = 0;
            foreach (var count in personel.izinlers.Where(x => x.izin_tur.tur.Equals("Ücretli izin") && x.izin_baslangic.Value.Year == DateTime.Now.Year && x.izin_bitis.Value.Year == DateTime.Now.Year && x.onay_tarihi != null).ToList())
                ucretliIzinCount += count.sure.HasValue ? count.sure : 0;
            var ucretliIzinStates = new States();
            ucretliIzinStates.remain = 10 - ucretliIzinCount;
            ucretliIzinStates.stateTitle = "Ücretli izin (" + ucretliIzinCount + "/10)";
            ucretliIzinStates.percentage = (ucretliIzinCount * 100) / 10;
            ucretliIzinStates.statePercentage = ucretliIzinStates.percentage + "%";
            int? ucretsizIzinCount = 0;
            foreach (var count in personel.izinlers.Where(x => x.izin_tur.tur.Equals("Ücretsiz izin") && x.izin_baslangic.Value.Year == DateTime.Now.Year && x.izin_bitis.Value.Year == DateTime.Now.Year && x.onay_tarihi != null).ToList())
                ucretsizIzinCount += count.sure.HasValue ? count.sure : 0;
            var ucretsizIzinStates = new States();
            ucretsizIzinStates.remain = 10 - ucretsizIzinCount;
            ucretsizIzinStates.stateTitle = "Ücretsiz izin (" + ucretsizIzinCount + "/10)";
            ucretsizIzinStates.percentage = (ucretsizIzinCount * 100) / 10;
            ucretsizIzinStates.statePercentage = ucretsizIzinStates.percentage + "%";
            int? mazeretsizGunCount = 0;
            foreach (var count in personel.izinlers.Where(x => x.izin_tur.tur.Equals("Mazeretsiz gelinmeyen gün") && x.izin_baslangic.Value.Year == DateTime.Now.Year && x.izin_bitis.Value.Year == DateTime.Now.Year && x.onay_tarihi != null).ToList())
                mazeretsizGunCount += count.sure.HasValue ? count.sure : 0;
            var mazeretsizGunStates = new States();
            mazeretsizGunStates.remain = 10 - mazeretsizGunCount;
            mazeretsizGunStates.stateTitle = "Mazeretsiz gelinmeyen gün (" + mazeretsizGunCount + "/10)";
            mazeretsizGunStates.percentage = (mazeretsizGunCount * 100) / 10;
            mazeretsizGunStates.statePercentage = mazeretsizGunStates.percentage + "%";
            izinStates = new List<States>();
            izinStates.Add(yillikIzinStates);
            izinStates.Add(raporluGunStates);
            izinStates.Add(ucretliIzinStates);
            izinStates.Add(ucretsizIzinStates);
            izinStates.Add(mazeretsizGunStates);
        }
    }

    public class States
    {
        public string stateTitle { get; set; }
        public string statePercentage { get; set; }
        public int? percentage { get; set; }
        public int? remain { get; set; }
    }
}