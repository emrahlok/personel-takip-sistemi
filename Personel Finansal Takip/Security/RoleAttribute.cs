using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Personel_Finansal_Takip.Models;
using System.Web.Security;

namespace System.Web.Mvc
{
    public class RoleAttribute : AuthorizeAttribute
    {
        public string UserRole { get; set; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            personeltakipsistemiEntities db = new personeltakipsistemiEntities();

            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }

            string CurrentUser = HttpContext.Current.User.Identity.Name.ToString();
            personel cookie_user = db.personels.Where(i => i.e_posta == CurrentUser && i.isten_cikis_tarihi == null ).FirstOrDefault();
            if (cookie_user != null)
            {
                var cookie_user_role = cookie_user.personel_rol.rol;
                if (UserRole.Equals(cookie_user_role))
                {
                    return true;
                }
            }
            FormsAuthentication.SignOut();
            return false;
            //return base.AuthorizeCore(httpContext);
        }
    }
}