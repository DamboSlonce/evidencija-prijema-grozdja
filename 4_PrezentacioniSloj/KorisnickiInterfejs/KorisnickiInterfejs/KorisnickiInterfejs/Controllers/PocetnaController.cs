using System.Web.Mvc;

namespace KorisnickiInterfejs.Controllers
{
    [ZahtevaPrijavu]
    public class PocetnaController : OsnovniKontroler
    {
        public ActionResult Index()
        {
            ViewBag.Korisnik = this.KorisnikImePrezime;
            return View();
        }

        public ActionResult OSistemu()
        {
            return View();
        }
    }
}
