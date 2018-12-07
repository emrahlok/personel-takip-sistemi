using Personel_Finansal_Takip.Areas.admin.Models;
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
    public class MaasController : Controller
    {
        personeltakipsistemiEntities db = new personeltakipsistemiEntities();
        // GET: admin/Maas
        public ActionResult Index()
        {
            var cookie_user = HttpContext.User.Identity.Name.ToString();
            var pmaas = db.maas.Where(x => x.personel.e_posta == cookie_user).ToList();
            return View(pmaas);
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
    }
}