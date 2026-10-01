using System.Web.Mvc;
using KorisnickiInterfejs.Models;
using PrezentacionaLogika;

namespace KorisnickiInterfejs.Controllers
{
    public class PrijavaController : OsnovniKontroler
    {
        public ActionResult Index()
        {
            return View(new PrijavaViewModel());
        }

        [HttpPost]
        public ActionResult Index(PrijavaViewModel model)
        {
            FormaLoginKlasa forma = new FormaLoginKlasa(this.StringKonekcije);
            forma.KorisnickoIme = model.KorisnickoIme;
            forma.Sifra = model.Sifra;
            bool pronadjen = forma.VazeciKorisnik();
            if (pronadjen)
            {
                Session["KorisnikImePrezime"] = forma.DajImePrezimeKorisnika();
                return RedirectToAction("Index", "Pocetna");
            }
            model.Poruka = "KORISNIK NIJE PRONADJEN!";
            return View(model);
        }

        public ActionResult Odjava()
        {
            Session.Clear();
            return RedirectToAction("Index", "Prijava");
        }

        public ActionResult OSistemu()
        {
            return View();
        }
    }
}
