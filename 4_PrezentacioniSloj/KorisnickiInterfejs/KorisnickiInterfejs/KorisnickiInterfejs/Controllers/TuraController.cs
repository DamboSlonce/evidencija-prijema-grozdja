using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using KorisnickiInterfejs.Models;
using PrezentacionaLogika;

namespace KorisnickiInterfejs.Controllers
{
    [ZahtevaPrijavu]
    public class TuraController : OsnovniKontroler
    {
        public ActionResult Spisak(string filter)
        {
            FormaTuraSpisakKlasa forma = new FormaTuraSpisakKlasa(this.StringKonekcije);
            DataSet podaci = forma.DajPodatkeZaGrid(this.NormalizujFilter(filter));
            ViewBag.Tabela = podaci.Tables[0];
            ViewBag.Filter = filter ?? "";
            return View();
        }

        [HttpPost]
        public ActionResult Spisak(TuraUnosViewModel model, string akcija)
        {
            if (akcija == "stampa")
            {
                string stampaFilter = string.IsNullOrEmpty(model.Filter) ? "0" : model.Filter;
                return RedirectToAction("Stampa", new { filter = stampaFilter });
            }
            string filter = "";
            if (akcija == "filtriraj")
            {
                filter = model.Filter ?? "";
            }
            return RedirectToAction("Spisak", new { filter = filter });
        }

        public ActionResult Unos()
        {
            this.NapuniCombo();
            return View(new TuraUnosViewModel());
        }

        [HttpPost]
        public ActionResult Unos(TuraUnosViewModel model)
        {
            this.NapuniCombo();
            FormaTuraUnosKlasa forma = new FormaTuraUnosKlasa(this.StringKonekcije);
            forma.Oznaka = model.Oznaka;
            forma.BrojPrijema = model.BrojPrijema;
            forma.DatumPrijema = model.DatumPrijema;
            forma.Isporucilac = model.Isporucilac;
            forma.NazivSorte = model.NazivSorte;
            forma.NetoMasaKg = model.NetoMasaKg;
            forma.SecerProcenat = model.SecerProcenat;
            forma.KiselostGl = model.KiselostGl;
            forma.TemperaturaC = model.TemperaturaC;
            forma.UdeoOstecenihProcenat = model.UdeoOstecenihProcenat;
            if (model.Usmerenje == "Izaberite...")
            {
                forma.Usmerenje = "";
            }
            else
            {
                forma.Usmerenje = model.Usmerenje;
            }

            if (model.NazivSorte == "Izaberite...")
            {
                model.Poruka = "NISTE IZABRALI NAZIV SORTE!";
                return View(model);
            }

            try
            {
                model.Poruka = forma.SnimiPodatke();
            }
            catch (Exception greska)
            {
                model.Poruka = "GRESKA:" + greska.Message;
            }
            return View(model);
        }

        public ActionResult Stampa(string filter)
        {
            string parametar = this.NormalizujFilter(filter);
            FormaTuraStampaKlasa forma = new FormaTuraStampaKlasa(this.StringKonekcije);
            ViewBag.Tabela = forma.DajPodatkeZaGrid(parametar).Tables[0];
            if (string.IsNullOrEmpty(parametar))
            {
                ViewBag.Naslov = "ZAPISNIK O PRIJEMU GROZDA - SVE TURE";
            }
            else
            {
                ViewBag.Naslov = "ZAPISNIK O PRIJEMU GROZDA, isporucilac=" + parametar;
            }
            return View();
        }

        public ActionResult ParametarStampe()
        {
            return View(new TuraUnosViewModel());
        }

        [HttpPost]
        public ActionResult ParametarStampe(TuraUnosViewModel model)
        {
            string filter = string.IsNullOrEmpty(model.Filter) ? "0" : model.Filter;
            return RedirectToAction("Stampa", new { filter = filter });
        }

        private void NapuniCombo()
        {
            ViewBag.Sorte = this.DajStavkeSorti();
            List<SelectListItem> usmerenja = new List<SelectListItem>();
            usmerenja.Add(new SelectListItem { Text = "Izaberite...", Value = "Izaberite..." });
            usmerenja.Add(new SelectListItem { Text = "kvalitetno vino", Value = "kvalitetno vino" });
            usmerenja.Add(new SelectListItem { Text = "destilat", Value = "destilat" });
            ViewBag.Usmerenja = usmerenja;
        }
    }
}
