using Personel_Finansal_Takip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Personel_Finansal_Takip.Areas.manager.Controllers
{
    [Role(UserRole = "manager")]
    public class MyInformationController : Controller
    {
        personeltakipsistemiEntities db = new personeltakipsistemiEntities();
        // GET: manager/MyInformation
        public ActionResult Index()
        {
            var cookie_user = HttpContext.User.Identity.Name.ToString();
            var user = db.personels.Where(acc => acc.e_posta == cookie_user).FirstOrDefault();
            return View(user);
        }
    }
}