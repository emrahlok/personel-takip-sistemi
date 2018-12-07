using Personel_Finansal_Takip.Models;
using Rotativa;
using Rotativa.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Personel_Finansal_Takip.Areas.employee.Controllers
{
    public class PuantajCetveliController : Controller
    {
        personeltakipsistemiEntities db = new personeltakipsistemiEntities();
        // GET: employee/PuantajCetveli
        public ActionResult Index()
        {
            var cookie_user = HttpContext.User.Identity.Name.ToString();
            var ppuantaj = db.personel_puantaj.Where(x => x.personel.e_posta == cookie_user).ToList();
            return View(ppuantaj);
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
    }
}