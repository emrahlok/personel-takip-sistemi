using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Personel_Finansal_Takip.Models;
using System.Web.Security;
using Rotativa;

namespace Personel_Finansal_Takip.Controllers
{
    public class HomeController : Controller
    {
        personeltakipsistemiEntities db = new personeltakipsistemiEntities();

        public ActionResult Login()
        {
            var cookie = HttpContext.User.Identity.Name.ToString();
            var cookie_user = db.personels.Where(user => user.e_posta == cookie).FirstOrDefault();
            if (FormsAuthentication.FormsCookieName != null && cookie_user != null)
            {
                var user_role = cookie_user.personel_rol.rol;
                switch (user_role)
                {
                    case "admin":
                        return RedirectToAction("", "admin");
                        break;
                    case "manager":
                        return RedirectToAction("", "manager");
                        break;
                    case "employee":
                        return RedirectToAction("", "employee");
                        break;
                    default:
                        break;
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection fCollection)
        {
            var email = fCollection["email"];
            var password = fCollection["password"];
            var personel = db.personels.Where(prsnl => prsnl.e_posta.Equals(email) && prsnl.sifre.Equals(password) && prsnl.isten_cikis_tarihi == null).FirstOrDefault();
            ViewBag.LoginError = null;
            if (personel != null)
            {
                FormsAuthentication.SetAuthCookie(email, false);
                switch (personel.personel_rol.rol)
                {
                    case "admin":
                        return RedirectToAction("", "admin");
                        break;
                    case "manager":
                        return RedirectToAction("", "manager");
                        break;
                    case "employee":
                        return RedirectToAction("", "employee");
                        break;
                    default:
                        break;
                }
                ViewBag.LoginError = "Çalışan kullanıcı düzeyi doğrulanamadı.";
                return View();
            }
            ViewBag.LoginError = "E-mail ve şifre doğrulanamadı.";
            return View();
        }

        public ActionResult PrintLogin()
        {
            return new ActionAsPdf("Login") { FileName = "Test.pdf" };
        }
    }
}