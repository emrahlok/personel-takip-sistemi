using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Personel_Finansal_Takip.Models;

namespace Personel_Finansal_Takip.Areas.employee.Controllers
{
    [Role(UserRole = "employee")]
    public class FazlaMesaiController : Controller
    {
        personeltakipsistemiEntities db = new personeltakipsistemiEntities();
        // GET: employee/FazlaMesai
        public ActionResult Index()
        {
            var cookie_user = HttpContext.User.Identity.Name.ToString();
            var user = db.personels.Where(acc => acc.e_posta == cookie_user).FirstOrDefault();
            return View(user.fazla_mesai.ToList());
        }
    }
}