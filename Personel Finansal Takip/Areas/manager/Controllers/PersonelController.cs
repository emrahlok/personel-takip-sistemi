using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Personel_Finansal_Takip.Models;

namespace Personel_Finansal_Takip.Areas.manager.Controllers
{
    [Role(UserRole = "manager")]
    public class PersonelController : Controller
    {
        personeltakipsistemiEntities db = new personeltakipsistemiEntities();
        // GET: manager/Personel
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TumDetaylar(int? Id)
        {
            return View(db.personels.Find(Id));
        }
    }
}