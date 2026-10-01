using System.Web.Mvc;
using System.Web.Routing;

namespace KorisnickiInterfejs
{
    public class RutaKonfiguracija
    {
        public static void RegistrujRute(RouteCollection rute)
        {
            rute.IgnoreRoute("{resource}.axd/{*pathInfo}");
            rute.IgnoreRoute("{resource}.aspx/{*pathInfo}");
            rute.IgnoreRoute("{resource}.Master/{*pathInfo}");
            rute.IgnoreRoute("{resource}.ashx/{*pathInfo}");

            rute.MapRoute(
                "Podrazumevano",
                "{controller}/{action}/{id}",
                new { controller = "Prijava", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
