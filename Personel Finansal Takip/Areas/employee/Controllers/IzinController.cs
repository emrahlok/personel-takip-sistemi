using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Personel_Finansal_Takip.Models;
using Personel_Finansal_Takip.Areas.employee.Models;
using System.Net;
using System.Data.Entity;

namespace Personel_Finansal_Takip.Areas.employee.Controllers
{
    [Role(UserRole = "employee")]
    public class IzinController : Controller
    {
        personeltakipsistemiEntities db = new personeltakipsistemiEntities();
        // GET: employee/Izin
        public ActionResult Index()
        {
            var cookie_user = HttpContext.User.Identity.Name.ToString();
            var asdf = new IzinViewModel();
            asdf.izinState = new IzinStateView(cookie_user);
            asdf.pIzinler = db.izinlers.Where(x => x.personel.e_posta.Equals(cookie_user)).ToList();
            return View(asdf);
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
            ViewBag.tur_id = new SelectList(db.izin_tur.Where(a => a.tur.Contains("izin")).ToList().Select(
                x => new SelectListItem
                {
                    Text = x.tur.ToString(),
                    Value = x.Id.ToString()
                }), "Value", "Text", izinler.tur_id);
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

        public ActionResult Talep()
        {
            var cookie_user = HttpContext.User.Identity.Name.ToString();
            izinler izinler = new izinler();
            izinler.personel_id = db.personels.Where(x => x.e_posta.Equals(cookie_user)).FirstOrDefault().id;
            ViewBag.tur_id = new SelectList(db.izin_tur.Where(a => a.tur.Contains("izin")).ToList().Select(
                x => new SelectListItem
                {
                    Text = x.tur.ToString(),
                    Value = x.Id.ToString()
                }), "Value", "Text");
            return View(izinler);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Talep([Bind(Include = "id,personel_id,talep_tarihi,onay_tarihi,son_degisiklik,tur_id,izin_baslangic,izin_bitis,ise_baslangic,sure,yol_izni,izinde_bul_adres,aciklama")] izinler izinler)
        {
            if (ModelState.IsValid)
            {
                izinler.red_durumu = false;
                izinler.son_degisiklik = DateTime.Now;
                izinler.talep_tarihi = DateTime.Now;
                db.izinlers.Add(izinler);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.tur_id = new SelectList(db.izin_tur.Where(a => a.tur.Contains("izin")).ToList().Select(
                x => new SelectListItem
                {
                    Text = x.tur.ToString(),
                    Value = x.Id.ToString()
                }), "Value", "Text", izinler.tur_id);
            return View(izinler);
        }
    }
}