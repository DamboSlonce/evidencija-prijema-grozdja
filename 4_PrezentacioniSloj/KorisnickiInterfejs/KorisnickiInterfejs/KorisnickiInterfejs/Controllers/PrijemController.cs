using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using KorisnickiInterfejs.Models;
using PrezentacionaLogika;

namespace KorisnickiInterfejs.Controllers
{
    [ZahtevaPrijavu]
    public class PrijemController : OsnovniKontroler
    {
        public ActionResult Unos()
        {
            Session["StavkePrijema"] = null;
            this.NapuniCombo();
            ViewBag.Stavke = this.DajStavkeTabelu();
            return View(new PrijemUnosViewModel());
        }

        [HttpPost]
        public ActionResult Unos(PrijemUnosViewModel model, string akcija)
        {
            this.NapuniCombo();
            if (akcija == "ponisti")
            {
                Session["StavkePrijema"] = null;
                ViewBag.Stavke = this.DajStavkeTabelu();
                return View(new PrijemUnosViewModel());
            }

            if (akcija == "dodaj")
            {
                DataTable tabela = this.DajStavkeTabelu();
                DataRow red = tabela.NewRow();
                red["Oznaka"] = model.Oznaka;
                red["NazivSorte"] = model.NazivSorte;
                red["NetoMasaKg"] = model.NetoMasaKg;
                red["SecerProcenat"] = model.SecerProcenat;
                red["KiselostGl"] = model.KiselostGl;
                red["TemperaturaC"] = model.TemperaturaC;
                red["UdeoOstecenihProcenat"] = model.UdeoOstecenihProcenat;
                tabela.Rows.Add(red);
                model.Oznaka = "";
                model.NazivSorte = "Izaberite...";
                model.NetoMasaKg = "";
                model.SecerProcenat = "";
                model.KiselostGl = "";
                model.TemperaturaC = "";
                model.UdeoOstecenihProcenat = "";
                model.Poruka = "Tura dodata u prijem. Snimite sve da se upise u bazu.";
                ViewBag.Stavke = tabela;
                return View(model);
            }

            FormaPrijemUnosKlasa forma = new FormaPrijemUnosKlasa(this.StringKonekcije);
            forma.BrojPrijema = model.BrojPrijema;
            forma.DatumPrijema = model.DatumPrijema;
            forma.Isporucilac = model.Isporucilac;
            try
            {
                model.Poruka = forma.SnimiPrijemSaTabelom(this.DajStavkeTabelu());
            }
            catch (Exception greska)
            {
                model.Poruka = "GRESKA:" + greska.Message;
            }
            ViewBag.Stavke = this.DajStavkeTabelu();
            return View(model);
        }

        public ActionResult Prikaz()
        {
            this.NapuniPrijeme();
            ViewBag.Ture = null;
            return View(new PrijemUnosViewModel());
        }

        [HttpPost]
        public ActionResult Prikaz(PrijemUnosViewModel model)
        {
            this.NapuniPrijeme();
            if (string.IsNullOrEmpty(model.IzabraniBrojPrijema) || model.IzabraniBrojPrijema == "Izaberite...")
            {
                ViewBag.Ture = null;
                return View(model);
            }
            FormaPrijemPrikazKlasa forma = new FormaPrijemPrikazKlasa(this.StringKonekcije);
            ViewBag.Ture = forma.DajTurePoBrojuPrijema(model.IzabraniBrojPrijema).Tables[0];
            return View(model);
        }

        private DataTable DajStavkeTabelu()
        {
            if (Session["StavkePrijema"] == null)
            {
                DataTable tabela = new DataTable();
                tabela.Columns.Add("Oznaka");
                tabela.Columns.Add("NazivSorte");
                tabela.Columns.Add("NetoMasaKg");
                tabela.Columns.Add("SecerProcenat");
                tabela.Columns.Add("KiselostGl");
                tabela.Columns.Add("TemperaturaC");
                tabela.Columns.Add("UdeoOstecenihProcenat");
                Session["StavkePrijema"] = tabela;
            }
            return (DataTable)Session["StavkePrijema"];
        }

        private void NapuniCombo()
        {
            ViewBag.Sorte = this.DajStavkeSorti();
        }

        private void NapuniPrijeme()
        {
            FormaPrijemPrikazKlasa forma = new FormaPrijemPrikazKlasa(this.StringKonekcije);
            DataSet prijemi = forma.DajSvePrijeme();
            List<SelectListItem> lista = new List<SelectListItem>();
            lista.Add(new SelectListItem { Text = "Izaberite...", Value = "Izaberite..." });
            if (prijemi != null && prijemi.Tables.Count > 0)
            {
                for (int i = 0; i < prijemi.Tables[0].Rows.Count; i++)
                {
                    string broj = prijemi.Tables[0].Rows[i]["BrojPrijema"].ToString();
                    string isporucilac = prijemi.Tables[0].Rows[i]["Isporucilac"].ToString();
                    lista.Add(new SelectListItem { Text = broj + " — " + isporucilac, Value = broj });
                }
            }
            ViewBag.Prijemi = lista;
        }
    }
}
