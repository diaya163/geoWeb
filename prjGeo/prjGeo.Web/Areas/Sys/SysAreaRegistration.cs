using System.Web.Mvc;

namespace prjGeo.Web.Areas.Sys
{
    public class SysAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Sys";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                this.AreaName+"default",
                this.AreaName+"/{controller}/{action}/{id}",
                new { area = this.AreaName, controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "prjGeo.Web.Areas." + this.AreaName + ".Controllers" }
            );
        }
    }
}
