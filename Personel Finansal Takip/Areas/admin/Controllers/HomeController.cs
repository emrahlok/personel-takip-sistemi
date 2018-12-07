using Rotativa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Personel_Finansal_Takip.Areas.admin.Controllers
{
    [Role(UserRole = "admin")]
    public class HomeController : Controller
    {
        // GET: admin/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        public ActionResult PrintIndex()
        {
            return new ActionAsPdf("Index") { FileName = "Admin.pdf" };
        }
    }
}