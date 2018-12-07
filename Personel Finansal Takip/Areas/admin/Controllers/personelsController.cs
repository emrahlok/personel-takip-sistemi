using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Personel_Finansal_Takip.Models;

namespace Personel_Finansal_Takip.Areas.admin.Controllers
{
    public class personelsController : Controller
    {
        private personeltakipsistemiEntities db = new personeltakipsistemiEntities();

        // GET: admin/personels
        public async Task<ActionResult> Index()
        {
            var personel = db.personel.Include(p => p.gunler).Include(p => p.personel_rol);
            return View(await personel.ToListAsync());
        }

        // GET: admin/personels/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            personel personel = await db.personel.FindAsync(id);
            if (personel == null)
            {
                return HttpNotFound();
            }
            return View(personel);
        }

        // GET: admin/personels/Create
        public ActionResult Create()
        {
            ViewBag.haftalik_izin_gun = new SelectList(db.gunler, "Id", "gun");
            ViewBag.rol_id = new SelectList(db.personel_rol, "Id", "rol");
            return View();
        }

        // POST: admin/personels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,ad,soyad,ilk_soyad,tc_kimlik_no,cinsiyet,dogum_tarihi,dogum_yeri,baba_adi,anne_adi,ise_giris_tarihi,isten_cikis_tarihi,ssk_no,statu,medeni_hal,cocuk_sayisi,departman,gorev,tahsil,meslek,ev_tel,is_tel,cep_tel,e_posta,sifre,rol_id,adres,adres_il,adres_ilce,kan_grubu,nufus_seri_no,nufus_no,nfs_kayitli_il,nfs_kayitli_ilce,nfs_kytli_mah_koy,cilt_no,aile_sira_no,sira_no,vergi_no,haftalik_izin_gun,aciklama,ResimBoyutu,DosyaIsmi,ResimVeri,brut_maas,net_maas")] personel personel)
        {
            if (ModelState.IsValid)
            {
                db.personel.Add(personel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.haftalik_izin_gun = new SelectList(db.gunler, "Id", "gun", personel.haftalik_izin_gun);
            ViewBag.rol_id = new SelectList(db.personel_rol, "Id", "rol", personel.rol_id);
            return View(personel);
        }

        // GET: admin/personels/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            personel personel = await db.personel.FindAsync(id);
            if (personel == null)
            {
                return HttpNotFound();
            }
            ViewBag.haftalik_izin_gun = new SelectList(db.gunler, "Id", "gun", personel.haftalik_izin_gun);
            ViewBag.rol_id = new SelectList(db.personel_rol, "Id", "rol", personel.rol_id);
            return View(personel);
        }

        // POST: admin/personels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,ad,soyad,ilk_soyad,tc_kimlik_no,cinsiyet,dogum_tarihi,dogum_yeri,baba_adi,anne_adi,ise_giris_tarihi,isten_cikis_tarihi,ssk_no,statu,medeni_hal,cocuk_sayisi,departman,gorev,tahsil,meslek,ev_tel,is_tel,cep_tel,e_posta,sifre,rol_id,adres,adres_il,adres_ilce,kan_grubu,nufus_seri_no,nufus_no,nfs_kayitli_il,nfs_kayitli_ilce,nfs_kytli_mah_koy,cilt_no,aile_sira_no,sira_no,vergi_no,haftalik_izin_gun,aciklama,ResimBoyutu,DosyaIsmi,ResimVeri,brut_maas,net_maas")] personel personel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(personel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.haftalik_izin_gun = new SelectList(db.gunler, "Id", "gun", personel.haftalik_izin_gun);
            ViewBag.rol_id = new SelectList(db.personel_rol, "Id", "rol", personel.rol_id);
            return View(personel);
        }

        // GET: admin/personels/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            personel personel = await db.personel.FindAsync(id);
            if (personel == null)
            {
                return HttpNotFound();
            }
            return View(personel);
        }

        // POST: admin/personels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            personel personel = await db.personel.FindAsync(id);
            db.personel.Remove(personel);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
