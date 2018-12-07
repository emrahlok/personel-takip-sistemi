using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Personel_Finansal_Takip.Models
{
    public class LayoutModel
    {
        personeltakipsistemiEntities db = new personeltakipsistemiEntities();
        public personel user { get; set; }

        public LayoutModel()
        {
            var cookie_user = HttpContext.Current.User.Identity.Name.ToString();
            user = db.personels.Where(acc => acc.e_posta == cookie_user).FirstOrDefault();
        }
    }
}