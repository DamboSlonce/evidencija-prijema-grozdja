using System.Data;
using System.Web.Mvc;
using KorisnickiInterfejs.Models;
using PrezentacionaLogika;

namespace KorisnickiInterfejs.Controllers
{
    [ZahtevaPrijavu]
    public class SortaController : OsnovniKontroler
    {
        public ActionResult Tabelarni(string filter)
        {
            FormaSortaTabelaEditKlasa forma = new FormaSortaTabelaEditKlasa(this.StringKonekcije);
            DataSet podaci = forma.DajPodatkeZaGrid(this.NormalizujFilter(filter));
            ViewBag.Tabela = podaci.Tables[0];
            ViewBag.Filter = filter ?? "";
            return View();
        }

        [HttpPost]
        public ActionResult Tabelarni(SortaViewModel model, string akcija)
        {
            string filter = "";
            if (akcija == "filtriraj")
            {
                filter = model.Filter ?? "";
            }
            return RedirectToAction("Tabelarni", new { filter = filter });
        }

        public ActionResult Unos()
        {
            return View(new SortaViewModel());
        }

        [HttpPost]
        public ActionResult Unos(SortaViewModel model)
        {
            FormaSortaUnosKlasa forma = new FormaSortaUnosKlasa(this.StringKonekcije);
            forma.Sifra = model.Sifra;
            forma.Naziv = model.Naziv;
            bool uspeh = forma.SnimiPodatke();
            model.Poruka = uspeh ? "Uspesno snimljeni novi podaci!" : "GRESKA SNIMANJA PODATAKA!";
            return View(model);
        }

        public ActionResult Izmena()
        {
            FormaSortaTabelaEditKlasa forma = new FormaSortaTabelaEditKlasa(this.StringKonekcije);
            ViewBag.Tabela = forma.DajPodatkeZaGrid("").Tables[0];
            return View();
        }

        public ActionResult Detalji(string Sifra)
        {
            FormaSortaDetaljiEditKlasa forma = new FormaSortaDetaljiEditKlasa(this.StringKonekcije);
            forma.SifraPreuzeteSorte = Sifra;
            SortaViewModel model = new SortaViewModel();
            model.Sifra = forma.SifraPreuzeteSorte;
            model.Naziv = forma.NazivPreuzeteSorte;
            model.SifraPreuzete = forma.SifraPreuzeteSorte;
            return View(model);
        }

        [HttpPost]
        public ActionResult Obrisi(SortaViewModel model)
        {
            FormaSortaDetaljiEditKlasa forma = new FormaSortaDetaljiEditKlasa(this.StringKonekcije);
            forma.SifraPreuzeteSorte = model.SifraPreuzete;
            bool uspeh = forma.ObrisiSortu();
            model.Poruka = uspeh ? "Uspesno obrisan zapis!" : "NEUSPEH BRISANJA zapisa!";
            if (uspeh)
            {
                model.Sifra = "";
                model.Naziv = "";
            }
            return View("Detalji", model);
        }

        [HttpPost]
        public ActionResult SnimiIzmenu(SortaViewModel model)
        {
            FormaSortaDetaljiEditKlasa forma = new FormaSortaDetaljiEditKlasa(this.StringKonekcije);
            forma.SifraPreuzeteSorte = model.SifraPreuzete;
            forma.SifraIzmenjeneSorte = model.Sifra;
            forma.NazivIzmenjeneSorte = model.Naziv;
            bool uspeh = forma.IzmeniSortu();
            model.Poruka = uspeh ? "Uspesno izmenjen zapis!" : "NEUSPEH IZMENE zapisa!";
            return View("Detalji", model);
        }

        public ActionResult Stampa(string filter)
        {
            string parametar = this.NormalizujFilter(filter);
            FormaSortaStampaKlasa forma = new FormaSortaStampaKlasa(this.StringKonekcije);
            ViewBag.Tabela = forma.DajPodatkeZaGrid(parametar).Tables[0];
            if (string.IsNullOrEmpty(parametar))
            {
                ViewBag.Naslov = "SPISAK SVIH SORTI";
            }
            else
            {
                ViewBag.Naslov = "FILTRIRANI SPISAK SORTI, naziv=" + parametar;
            }
            return View();
        }

        public ActionResult ParametarStampe()
        {
            return View(new SortaViewModel());
        }

        [HttpPost]
        public ActionResult ParametarStampe(SortaViewModel model)
        {
            string filter = string.IsNullOrEmpty(model.Filter) ? "0" : model.Filter;
            return RedirectToAction("Stampa", new { filter = filter });
        }
    }
}
