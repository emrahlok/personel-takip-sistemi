using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Personel_Finansal_Takip.Models;
using System.IO;
using System.Data.Entity;
using Rotativa;
using Rotativa.Options;

namespace Personel_Finansal_Takip.Areas.admin.Controllers
{
    [Role(UserRole = "admin")]
    public class EmployeesController : Controller
    {
        private personeltakipsistemiEntities db = new personeltakipsistemiEntities();
        private static byte[] EmployeeImageData = null;
        // GET: admin/Employees
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(int personel_id)
        {
            EmployeeImageData = null;
            var personel = db.personels.Find(personel_id);
            ViewBag.haftalik_izin_gun = new SelectList(db.gunlers, "Id", "gun", personel.haftalik_izin_gun);
            ViewBag.rol_id = new SelectList(db.personel_rol, "Id", "rol", personel.rol_id);
            return View(personel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(personel personel)
        {
            if (ModelState.IsValid)
            {
                if (EmployeeImageData != null)
                {
                    personel.ResimVeri = EmployeeImageData;
                    EmployeeImageData = null;
                }
                db.Entry(personel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.haftalik_izin_gun = new SelectList(db.gunlers, "Id", "gun", personel.haftalik_izin_gun);
            ViewBag.rol_id = new SelectList(db.personel_rol, "Id", "rol", personel.rol_id);
            return View(personel);
        }

        public ActionResult Create()
        {
            EmployeeImageData = null;
            ViewBag.haftalik_izin_gun = new SelectList(db.gunlers, "Id", "gun");
            ViewBag.rol_id = new SelectList(db.personel_rol, "Id", "rol");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(personel personel)
        {
            if (ModelState.IsValid)
            {
                if (EmployeeImageData != null)
                {
                    personel.ResimVeri = EmployeeImageData;
                    EmployeeImageData = null;
                }
                db.personels.Add(personel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.haftalik_izin_gun = new SelectList(db.gunlers, "Id", "gun", personel.haftalik_izin_gun);
            ViewBag.rol_id = new SelectList(db.personel_rol, "Id", "rol", personel.rol_id);
            return View(personel);
        }

        public ActionResult Delete(int personel_id)
        {
            var personel = db.personels.Find(personel_id);
            db.personels.Remove(personel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult IstenCikar(int personel_id)
        {
            var personel = db.personels.Find(personel_id);
            personel.isten_cikis_tarihi = DateTime.Now.Date;
            db.Entry(personel).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult TekrarIseAl(int personel_id)
        {
            var personel = db.personels.Find(personel_id);
            personel.isten_cikis_tarihi = null;
            db.Entry(personel).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public void FotoCek()
        {
            var stream = Request.InputStream;
            string dump;
            using (var reader = new StreamReader(stream))
            {
                dump = reader.ReadToEnd();
                DateTime dateTime = DateTime.Now;
                EmployeeImageData = StringToBytes(dump);
            }
        }

        private byte[] StringToBytes(string strInput)
        {
            int numBytes = (strInput.Length) / 2;
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(strInput);
            //byte[] bytes = new byte[numBytes];
            //for (int x = 0; x < numBytes; ++x)
            //{
            //    bytes[x] = Convert.ToByte(strInput.Substring(x * 2, 2), 16);
            //}
            return bytes;
        }

        public ActionResult Test()
        {
            return View();
        }

        //public void ImageData(string imageSource)
        //{
        //    EmployeeImageData = StringToBytes(imageSource);
        //}

        [HttpPost]
        public JsonResult ImageData(string imageSource)
        {
            EmployeeImageData = StringToBytes(imageSource);
            return Json(System.Text.Encoding.ASCII.GetString(EmployeeImageData));
        }

        public void DeletePhoto()
        {
            EmployeeImageData = null;
        }

        public ActionResult PrintIndex()
        {
            return new ActionAsPdf("Index")
            {
                FileName = "TestView.pdf",
                PageSize = Size.A4,
                PageOrientation = Orientation.Landscape,
                PageMargins = { Top = 0, Right = 0, Bottom = 0, Left = 0 }
            };
        }
    }
}