using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Personel_Finansal_Takip.Models;
using Personel_Finansal_Takip.Areas.admin.Models;
using System.Data.Entity;

namespace Personel_Finansal_Takip.Areas.admin.Controllers
{
    [Role(UserRole = "admin")]
    public class FazlaMesaiController : Controller
    {
        personeltakipsistemiEntities db = new personeltakipsistemiEntities();
        // GET: admin/FazlaMesai
        public ActionResult Index()
        {
            return View(db.fazla_mesai.ToList());
        }

        public ActionResult Edit(int Id)
        {
            var bulunan = db.fazla_mesai.Find(Id);
            ViewBag.personel_id = new SelectList(db.personels.ToList().Select(
                x => new SelectListItem
                {
                    Text = x.ad + " " + x.soyad,
                    Value = x.id.ToString()
                }), "Value", "Text", bulunan.personel_id);
            return View(bulunan);
        }

        [HttpPost]
        public ActionResult Edit(fazla_mesai fazla_mesai)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fazla_mesai).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.personel_id = new SelectList(db.personels.ToList().Select(
                x => new SelectListItem
                {
                    Text = x.ad + " " + x.soyad,
                    Value = x.id.ToString()
                }), "Value", "Text", fazla_mesai.personel_id);
            return View(fazla_mesai);
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
        public ActionResult Create(fazla_mesai fazla_mesai)
        {
            if (ModelState.IsValid)
            {
                db.fazla_mesai.Add(fazla_mesai);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.personel_id = new SelectList(db.personels.ToList().Select(
                x => new SelectListItem
                {
                    Text = x.ad + " " + x.soyad,
                    Value = x.id.ToString()
                }), "Value", "Text", fazla_mesai.personel_id);
            return View(fazla_mesai);
        }

        public ActionResult Delete(int Id)
        {
            var silinecek = db.fazla_mesai.Find(Id);
            db.fazla_mesai.Remove(silinecek);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //[HttpPost]
        //public ActionResult Index(FazlaMesaiViewModel fm)
        //{
        //    //ViewBag.personel_id = new SelectList(db.personel.ToList().Select(
        //    //    x => new SelectListItem
        //    //    {
        //    //        Text = x.ad + " " + x.soyad,
        //    //        Value = x.id.ToString()
        //    //    }), "Value", "Text");
        //    //new DateTime().

        //    return View(new FazlaMesaiViewModel());
        //}
    }
}