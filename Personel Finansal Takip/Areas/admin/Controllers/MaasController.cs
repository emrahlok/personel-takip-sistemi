using Personel_Finansal_Takip.Areas.admin.Models;
using Personel_Finansal_Takip.Models;
using Rotativa;
using Rotativa.Options;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Personel_Finansal_Takip.Areas.admin.Controllers
{
    public class MaasController : Controller
    {
        personeltakipsistemiEntities db = new personeltakipsistemiEntities();
        // GET: admin/Maas
        public ActionResult Index()
        {
            return View(db.maas.ToList());
        }

        public PartialViewResult GetBordroView(int maas_id)
        {
            BordroPartialModel asdf = new BordroPartialModel();
            asdf.personelmaas = db.maas.Find(maas_id);
            asdf.ppuantaj = db.personel_puantaj.Where(x => x.ay_yil == asdf.personelmaas.ay_yil && x.personel_id == asdf.personelmaas.personel_id).FirstOrDefault();
            return PartialView("MaasPartialView", asdf);
        }

        public ActionResult PrintMaasView(int Id)
        {
            BordroPartialModel asdf = new BordroPartialModel();
            asdf.personelmaas = db.maas.Find(Id);
            asdf.ppuantaj = db.personel_puantaj.Where(x => x.ay_yil == asdf.personelmaas.ay_yil && x.personel_id == asdf.personelmaas.personel_id).FirstOrDefault();
            return new PartialViewAsPdf("MaasBordro", asdf)
            {
                FileName = "Bordro-" + asdf.ppuantaj.personel.ad + "-" + asdf.ppuantaj.personel.soyad + "-" + asdf.personelmaas.ay_yil.Value.ToString("yyyy-MMMM") + ".pdf",
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
        public ActionResult Create(MaasCreateModel MCModel)
        {
            maas pmaas = new maas();
            pmaas.personel_id = MCModel.personel_id;
            pmaas.ay_yil = new DateTime(MCModel.yil, MCModel.ay, 1);
            personel_puantaj ppuantaj = db.personel_puantaj.Where(x => x.ay_yil == pmaas.ay_yil && x.personel_id == pmaas.personel_id).FirstOrDefault();
            pmaas.ucret_gun_sayisi = ppuantaj.ucret_gun;
            pmaas.prim_gunu_sayisi = ppuantaj.sigorta_gun;
            pmaas.prim_saati = ppuantaj.sigorta_saat;
            pmaas.gun_brut_ucreti = ppuantaj.personel.brut_maas / pmaas.prim_gunu_sayisi;
            var saatlikbrut = ppuantaj.personel.brut_maas / 225;
            pmaas.fazla_mesai = saatlikbrut * ppuantaj.fazla_mesai_saat;
            pmaas.sair_odeme = 0.00;
            pmaas.brut_toplam = ppuantaj.personel.brut_maas + pmaas.fazla_mesai + pmaas.sair_odeme;
            pmaas.vergi_matrahi = pmaas.brut_toplam * 85 / 100;
            pmaas.sigorta_matrahi = pmaas.brut_toplam;
            pmaas.damga_vergisi = pmaas.brut_toplam * 759 / 100000;
            double? kumulatif_toplam = 0.00;
            for (DateTime a = new DateTime(MCModel.yil, 1, 1); a < pmaas.ay_yil; a = a.AddMonths(1))
            {
                maas tempmaas = null;
                tempmaas = db.maas.Where(x => x.ay_yil == a && x.personel_id == pmaas.personel_id).FirstOrDefault();
                if (tempmaas != null)
                    kumulatif_toplam = kumulatif_toplam + tempmaas.vergi_matrahi;
            }
            if (kumulatif_toplam <= 12600)
            {
                pmaas.gelir_vergisi = pmaas.vergi_matrahi * 15 / 100;
            }
            else if ((kumulatif_toplam > 12600) && (kumulatif_toplam < 30000))
            {
                if ((kumulatif_toplam - 12600) < pmaas.vergi_matrahi)
                {
                    double? yuzde_yirmi = kumulatif_toplam - 12600;
                    double? yuzde_onbes = pmaas.vergi_matrahi - yuzde_yirmi;
                    pmaas.gelir_vergisi = (yuzde_onbes * 15 / 100) + (yuzde_yirmi * 20 / 100);
                }
                else
                {
                    pmaas.gelir_vergisi = pmaas.vergi_matrahi * 20 / 100;
                }
            }
            else if ((kumulatif_toplam > 30000) && (kumulatif_toplam < 110000))
            {
                if ((kumulatif_toplam - 30000) < pmaas.vergi_matrahi)
                {
                    double? yuzde_yirmiyedi = kumulatif_toplam - 30000;
                    double? yuzde_yirmi = pmaas.vergi_matrahi - yuzde_yirmiyedi;
                    pmaas.gelir_vergisi = (yuzde_yirmi * 20 / 100) + (yuzde_yirmiyedi * 27 / 100);
                }
                else
                {
                    pmaas.gelir_vergisi = pmaas.vergi_matrahi * 27 / 100;
                }
            }
            else if (kumulatif_toplam > 110000)
            {
                if ((kumulatif_toplam - 110000) < pmaas.vergi_matrahi)
                {
                    double? yuzde_otuzbes = kumulatif_toplam - 110000;
                    double? yuzde_yirmiyedi = pmaas.vergi_matrahi - yuzde_otuzbes;
                    pmaas.gelir_vergisi = (yuzde_yirmiyedi * 27 / 100) + (yuzde_otuzbes * 35 / 100);
                }
                else
                {
                    pmaas.gelir_vergisi = pmaas.vergi_matrahi * 35 / 100;
                }
            }
            pmaas.asgari_gecim_indirimi = (pmaas.vergi_matrahi * 50 / 100) * 15 / 100;
            if (ppuantaj.personel.medeni_hal.Equals("Evli"))
            {
                pmaas.asgari_gecim_indirimi += (pmaas.vergi_matrahi * 10 / 100) * 15 / 100;
            }
            if (ppuantaj.personel.cocuk_sayisi > 0)
            {
                for (int cocuk = 1; cocuk <= ppuantaj.personel.cocuk_sayisi; cocuk++)
                {
                    if (cocuk == 1)
                        pmaas.asgari_gecim_indirimi += (pmaas.vergi_matrahi * 75 / 1000) * 15 / 100;
                    else if (cocuk == 2)
                        pmaas.asgari_gecim_indirimi += (pmaas.vergi_matrahi * 75 / 1000) * 15 / 100;
                    else if (cocuk == 3)
                        pmaas.asgari_gecim_indirimi += (pmaas.vergi_matrahi * 10 / 100) * 15 / 100;
                    else
                        pmaas.asgari_gecim_indirimi += (pmaas.vergi_matrahi * 5 / 100) * 15 / 100;
                }
            }
            pmaas.sigorta_kesintisi = pmaas.brut_toplam * 14 / 100;
            pmaas.issizlik_sigortasi_kesintisi = pmaas.brut_toplam / 100;
            pmaas.isveren_sgk_istisnasi = pmaas.brut_toplam * 5 / 100;
            pmaas.net_ucret = pmaas.brut_toplam - pmaas.sigorta_kesintisi - pmaas.issizlik_sigortasi_kesintisi - pmaas.gelir_vergisi - pmaas.damga_vergisi;
            pmaas.odenecek_ucret = pmaas.net_ucret + pmaas.asgari_gecim_indirimi;
            ppuantaj.brut_ucret = ppuantaj.personel.brut_maas;
            ppuantaj.net_ucret = pmaas.net_ucret;
            db.Entry(ppuantaj).State = EntityState.Modified;
            db.SaveChanges();
            db.maas.Add(pmaas);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}