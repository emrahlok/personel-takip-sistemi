using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Personel_Finansal_Takip.Models;
using Rotativa;
using Rotativa.Options;
using Personel_Finansal_Takip.Areas.admin.Models;

namespace Personel_Finansal_Takip.Areas.admin.Controllers
{
    public class PersonelPuantajController : Controller
    {
        personeltakipsistemiEntities db = new personeltakipsistemiEntities();
        // GET: admin/PersonelPuantaj
        public ActionResult Index()
        {
            return View(db.personel_puantaj.ToList());
        }

        public ActionResult Delete(int Id)
        {
            var silinecek = db.personel_puantaj.Find(Id);
            db.personel_puantaj.Remove(silinecek);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public PartialViewResult GetProgress()
        {
            return PartialView("ProgressView");
        }

        public PartialViewResult GetPuantajView(int puantaj_id)
        {
            return PartialView("PuantajPartialView", db.personel_puantaj.Find(puantaj_id));
        }

        public ActionResult PrintPuantajView(int Id)
        {
            var puantaj = db.personel_puantaj.Find(Id);
            return new PartialViewAsPdf("PuantajCetveli", puantaj)
            {
                FileName = "Puantaj-" + puantaj.personel.ad + "-" + puantaj.personel.soyad + "-" + puantaj.ay_yil.Value.ToString("yyyy-MMMM") + ".pdf",
                PageSize = Size.A4,
                PageOrientation = Orientation.Landscape,
                PageMargins = { Top = 0, Right = 0, Bottom = 0, Left = 0 }
            };
        }

        public ActionResult Create()
        {
            ViewBag.personel_id = new SelectList(db.personels.ToList().Select(
                x => new SelectListItem
                {
                    Text = x.ad + " " + x.soyad,
                    Value = x.id.ToString()
                }), "Value", "Text", null);
            return View();
        }

        [HttpPost]
        public ActionResult Create(PuantajCreateModel PCModel)
        {
            personel prsnl = db.personels.Find(PCModel.personel_id);
            personel_puantaj ppuantaj = new personel_puantaj();
            ppuantaj.personel_id = PCModel.personel_id;
            ppuantaj.ay_yil = new DateTime(PCModel.yil, PCModel.ay, 1);
            ppuantaj.calisilan_gun = 0;
            ppuantaj.hafta_tatili = 0;
            ppuantaj.genel_tatil = 0;
            ppuantaj.ucretli_izin = 0;
            ppuantaj.ucretsiz_izin = 0;
            ppuantaj.sihhi_izin = 0;
            ppuantaj.yillik_izin = 0;
            ppuantaj.mazeretsiz_izin = 0;
            ppuantaj.ucret_gun = DateTime.DaysInMonth(PCModel.yil, PCModel.ay);
            ppuantaj.sigorta_gun = 30;
            ppuantaj.fazla_mesai_saat = 0;
            for (int b = 1; b <= DateTime.DaysInMonth(PCModel.yil, PCModel.ay); b++)
            {
                puantaj_gunler pgun = new puantaj_gunler();
                pgun.gun = b;
                DateTime scanDate = new DateTime(PCModel.yil, PCModel.ay, b);
                fazla_mesai fmpersonel = db.fazla_mesai.Where(x => x.tarih == scanDate).FirstOrDefault();
                izinler izin = db.izinlers.Where(
                    c => (c.izin_baslangic <= scanDate && c.izin_bitis >= scanDate) &&
                    c.onay_tarihi != null).FirstOrDefault();
                resmi_tatil tatil_gunu = db.resmi_tatil.Where(i => i.tarih == scanDate).FirstOrDefault();

                if (tatil_gunu != null)
                {
                    ppuantaj.genel_tatil++;
                    if (fmpersonel != null)
                    {
                        ppuantaj.fazla_mesai_saat += (75 / 10) * (15 / 10);
                    }
                    if (tatil_gunu.resmi_tatil_tur.Equals("Ramazan Bayramı Arifesi") ||
                        tatil_gunu.resmi_tatil_tur.Equals("Ramazan Bayramı") ||
                        tatil_gunu.resmi_tatil_tur.Equals("Kurban Bayramı Arifesi") ||
                        tatil_gunu.resmi_tatil_tur.Equals("Kurban Bayramı"))
                    {
                        pgun.gun_pauntaj = "D";
                    }
                    else if (tatil_gunu.resmi_tatil_tur.Equals("Ulusal Egemenlik ve Çocuk Bayramı") ||
                      tatil_gunu.resmi_tatil_tur.Equals("Atatürk'ü Anma, Gençlik ve Spor Bayramı") ||
                      tatil_gunu.resmi_tatil_tur.Equals("Zafer Bayramı") ||
                      tatil_gunu.resmi_tatil_tur.Equals("Cumhuriyet Bayramı Arifesi") ||
                      tatil_gunu.resmi_tatil_tur.Equals("Cumhuriyet Bayramı"))
                    {
                        pgun.gun_pauntaj = "B";
                    }
                    else
                        pgun.gun_pauntaj = "T";
                }
                else if (izin != null)
                {
                    if (izin.izin_tur.tur.Equals("Yıllık izin"))
                    {
                        ppuantaj.yillik_izin++;
                        pgun.gun_pauntaj = "Y";
                    }
                    else if (izin.izin_tur.tur.Equals("Mazeretsiz gelinmeyen gün"))
                    {
                        ppuantaj.mazeretsiz_izin++;
                        ppuantaj.sigorta_gun--;
                        ppuantaj.ucret_gun--;
                        pgun.gun_pauntaj = "M";
                    }
                    else if (izin.izin_tur.tur.Equals("Sıhhi (hastalık) raporu"))
                    {
                        ppuantaj.sihhi_izin++;
                        ppuantaj.sigorta_gun--;
                        ppuantaj.ucret_gun--;
                        pgun.gun_pauntaj = "R";
                    }
                    else if (izin.izin_tur.tur.Equals("Ücretli izin"))
                    {
                        ppuantaj.ucretli_izin++;
                        ppuantaj.sigorta_gun--;
                        ppuantaj.ucret_gun--;
                        pgun.gun_pauntaj = "Ü";
                    }
                    else if (izin.izin_tur.tur.Equals("Ücretsiz izin"))
                    {
                        ppuantaj.ucretsiz_izin++;
                        pgun.gun_pauntaj = "Z";
                    }
                }
                else
                {
                    if (fmpersonel != null)
                    {
                        ppuantaj.fazla_mesai_saat += (int)fmpersonel.sure_saat * (15 / 10);
                    }
                    if (prsnl.haftalik_izin_gun == (int)scanDate.DayOfWeek)
                    {
                        ppuantaj.hafta_tatili++;
                        pgun.gun_pauntaj = "T";
                    }
                    else
                    {
                        ppuantaj.calisilan_gun++;
                        pgun.gun_pauntaj = "N";
                    }
                }
                ppuantaj.puantaj_gunler.Add(pgun);
            }
            ppuantaj.sigorta_saat = ppuantaj.sigorta_gun * 75 / 10;
            db.personel_puantaj.Add(ppuantaj);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}