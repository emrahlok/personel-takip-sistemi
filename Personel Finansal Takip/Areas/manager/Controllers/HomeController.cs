using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Personel_Finansal_Takip.Areas.manager.Controllers
{
    [Role(UserRole = "manager")]
    public class HomeController : Controller
    {
        // GET: manager/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}