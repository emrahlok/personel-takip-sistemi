using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Personel_Finansal_Takip.Models;
using Personel_Finansal_Takip.Areas.admin.Models;

namespace Personel_Finansal_Takip.Areas.admin.Controllers
{
    [Role(UserRole = "admin")]
    public class IzinController : Controller
    {
        private personeltakipsistemiEntities db = new personeltakipsistemiEntities();

        // GET: admin/Izin
        public ActionResult Index()
        {
            var izinler = db.izinlers.Include(i => i.izin_tur).Include(i => i.personel);
            return View(izinler.ToList());
        }

        // GET: admin/Izin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            izinler izinler = db.izinlers.Find(id);
            if (izinler == null)
            {
                return HttpNotFound();
            }
            return View(izinler);
        }

        // GET: admin/Izin/Create
        public ActionResult Create()
        {
            ViewBag.tur_id = new SelectList(db.izin_tur, "Id", "tur");
            ViewBag.personel_id = new SelectList(db.personels.ToList().Select(
                x => new SelectListItem
                {
                    Text = x.ad + " " + x.soyad,
                    Value = x.id.ToString()
                }), "Value", "Text");
            return View();
        }

        // POST: admin/Izin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,personel_id,talep_tarihi,onay_tarihi,son_degisiklik,tur_id,izin_baslangic,izin_bitis,ise_baslangic,sure,yol_izni,izinde_bul_adres,aciklama")] izinler izinler)
        {
            if (ModelState.IsValid)
            {
                izinler.red_durumu = false;
                izinler.talep_tarihi = DateTime.Now;
                izinler.onay_tarihi = DateTime.Now;
                izinler.son_degisiklik = DateTime.Now;
                db.izinlers.Add(izinler);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.tur_id = new SelectList(db.izin_tur, "Id", "tur", izinler.tur_id);
            ViewBag.personel_id = new SelectList(db.personels, "id", "ad", izinler.personel_id);
            return View(izinler);
        }

        // GET: admin/Izin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            izinler izinler = db.izinlers.Find(id);
            if (izinler == null)
            {
                return HttpNotFound();
            }
            ViewBag.tur_id = new SelectList(db.izin_tur, "Id", "tur", izinler.tur_id);
            ViewBag.personel_id = new SelectList(db.personels, "id", "ad", izinler.personel_id);
            return View(izinler);
        }

        // POST: admin/Izin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,personel_id,talep_tarihi,onay_tarihi,son_degisiklik,tur_id,izin_baslangic,izin_bitis,ise_baslangic,sure,yol_izni,izinde_bul_adres,aciklama")] izinler izinler)
        {
            if (ModelState.IsValid)
            {
                izinler.red_durumu = false;
                izinler.son_degisiklik = DateTime.Now;
                db.Entry(izinler).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.tur_id = new SelectList(db.izin_tur, "Id", "tur", izinler.tur_id);
            ViewBag.personel_id = new SelectList(db.personels, "id", "ad", izinler.personel_id);
            return View(izinler);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Red([Bind(Include = "id,personel_id,talep_tarihi,onay_tarihi,son_degisiklik,tur_id,izin_baslangic,izin_bitis,ise_baslangic,sure,yol_izni,izinde_bul_adres,aciklama")] izinler izinler)
        {
            if (ModelState.IsValid)
            {
                izinler.red_durumu = true;
                izinler.son_degisiklik = DateTime.Now;
                db.Entry(izinler).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.tur_id = new SelectList(db.izin_tur, "Id", "tur", izinler.tur_id);
            ViewBag.personel_id = new SelectList(db.personels, "id", "ad", izinler.personel_id);
            return View(izinler);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Onayla([Bind(Include = "id,personel_id,talep_tarihi,onay_tarihi,son_degisiklik,tur_id,izin_baslangic,izin_bitis,ise_baslangic,sure,yol_izni,izinde_bul_adres,aciklama")] izinler izinler)
        {
            if (ModelState.IsValid)
            {
                izinler.red_durumu = false;
                izinler.onay_tarihi = DateTime.Now;
                izinler.son_degisiklik = DateTime.Now;
                db.Entry(izinler).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.tur_id = new SelectList(db.izin_tur, "Id", "tur", izinler.tur_id);
            ViewBag.personel_id = new SelectList(db.personels, "id", "ad", izinler.personel_id);
            return View(izinler);
        }

        // GET: admin/Izin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            izinler izinler = db.izinlers.Find(id);
            if (izinler == null)
            {
                return HttpNotFound();
            }
            else
            {
                db.izinlers.Remove(izinler);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(izinler);
        }

        // POST: admin/Izin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            izinler izinler = db.izinlers.Find(id);
            db.izinlers.Remove(izinler);
            db.SaveChanges();
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

        public PartialViewResult GetIzinState(int? personel_id)
        {
            return PartialView("_IzinState", new IzinStatePartialView(personel_id));
        }
    }
}
