using System.Web.Mvc;

namespace Personel_Finansal_Takip.Areas.manager
{
    public class managerAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "manager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "manager_default",
                "manager/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}