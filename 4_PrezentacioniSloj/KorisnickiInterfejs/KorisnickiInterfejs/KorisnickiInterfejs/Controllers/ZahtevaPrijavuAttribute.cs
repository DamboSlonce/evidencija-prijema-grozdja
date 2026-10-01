using System.Web.Mvc;

namespace KorisnickiInterfejs.Controllers
{
    public class ZahtevaPrijavuAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session == null ||
                filterContext.HttpContext.Session["KorisnikImePrezime"] == null)
            {
                filterContext.Result = new RedirectResult("~/Prijava");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
