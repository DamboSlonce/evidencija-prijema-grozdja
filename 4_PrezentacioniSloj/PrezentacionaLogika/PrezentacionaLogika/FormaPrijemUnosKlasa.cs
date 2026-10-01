using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using KlasePodataka;
using PoslovnaLogika;

namespace PrezentacionaLogika
{
    public class FormaPrijemUnosKlasa
    {
        private string _stringKonekcije;
        private string _brojPrijema;
        private string _datumPrijema;
        private string _isporucilac;

        public string BrojPrijema
        {
            get { return _brojPrijema; }
            set { _brojPrijema = value; }
        }

        public string DatumPrijema
        {
            get { return _datumPrijema; }
            set { _datumPrijema = value; }
        }

        public string Isporucilac
        {
            get { return _isporucilac; }
            set { _isporucilac = value; }
        }

        public FormaPrijemUnosKlasa(string noviStringKonekcije)
        {
            _stringKonekcije = noviStringKonekcije;
        }

        public DataSet DajPodatkeZaCombo()
        {
            SortaRepositoryKlasa sortaRepo = new SortaRepositoryKlasa(this._stringKonekcije);
            return sortaRepo.DajSveSorte();
        }

        public bool DaLiJeZaglavljePopunjeno()
        {
            bool popunjeno = false;
            if ((this._brojPrijema != null) && (this._datumPrijema != null) && (this._isporucilac != null))
            {
                if ((this._brojPrijema.Length > 0) && (this._datumPrijema.Length > 0) && (this._isporucilac.Length > 0))
                {
                    popunjeno = true;
                }
            }
            return popunjeno;
        }

        public string SnimiPrijemSaTabelom(DataTable tabelaStavki)
        {
            List<TuraKlasa> listaTura = new List<TuraKlasa>();
            if (tabelaStavki != null)
            {
                for (int i = 0; i < tabelaStavki.Rows.Count; i++)
                {
                    TuraKlasa tura = new TuraKlasa();
                    tura.Oznaka = tabelaStavki.Rows[i]["Oznaka"].ToString();
                    tura.NetoMasaKg = decimal.Parse(tabelaStavki.Rows[i]["NetoMasaKg"].ToString().Replace(',', '.'), CultureInfo.InvariantCulture);
                    tura.SecerProcenat = decimal.Parse(tabelaStavki.Rows[i]["SecerProcenat"].ToString().Replace(',', '.'), CultureInfo.InvariantCulture);
                    tura.KiselostGl = decimal.Parse(tabelaStavki.Rows[i]["KiselostGl"].ToString().Replace(',', '.'), CultureInfo.InvariantCulture);
                    tura.TemperaturaC = decimal.Parse(tabelaStavki.Rows[i]["TemperaturaC"].ToString().Replace(',', '.'), CultureInfo.InvariantCulture);
                    tura.UdeoOstecenihProcenat = decimal.Parse(tabelaStavki.Rows[i]["UdeoOstecenihProcenat"].ToString().Replace(',', '.'), CultureInfo.InvariantCulture);
                    SortaKlasa sorta = new SortaKlasa();
                    sorta.Naziv = tabelaStavki.Rows[i]["NazivSorte"].ToString();
                    tura.Sorta = sorta;
                    listaTura.Add(tura);
                }
            }
            return this.SnimiPrijemSaTurama(listaTura);
        }

        public string SnimiPrijemSaTurama(List<TuraKlasa> listaTura)
        {
            string poruka = "";
            if (this.DaLiJeZaglavljePopunjeno() == false)
            {
                return "NIJE SVE POPUNJENO!";
            }
            if ((listaTura == null) || (listaTura.Count == 0))
            {
                return "NIJE DODATA NI JEDNA TURA!";
            }

            UsmerenjePreradeKlasa usmerenjeObjekat = new UsmerenjePreradeKlasa(this._stringKonekcije);
            SortaRepositoryKlasa sortaRepo = new SortaRepositoryKlasa(this._stringKonekcije);
            for (int i = 0; i < listaTura.Count; i++)
            {
                TuraKlasa tura = listaTura[i];
                tura.BrojPrijema = this._brojPrijema;
                tura.DatumPrijema = DateTime.Parse(this._datumPrijema);
                tura.Isporucilac = this._isporucilac;
                string pomNaziv = tura.Sorta.Naziv;
                string pomSifra = sortaRepo.DajSifruSortePoNazivu(pomNaziv);
                tura.Sorta.Sifra = pomSifra;
                tura.Usmerenje = usmerenjeObjekat.OdrediUsmerenje(pomNaziv, tura.SecerProcenat, tura.UdeoOstecenihProcenat);
            }

            PrijemKlasa prijem = new PrijemKlasa();
            prijem.BrojPrijema = this._brojPrijema;
            prijem.DatumPrijema = DateTime.Parse(this._datumPrijema);
            prijem.Isporucilac = this._isporucilac;

            PrijemRepositoryKlasa prijemRepo = new PrijemRepositoryKlasa(this._stringKonekcije);
            bool uspeh = prijemRepo.SnimiPrijemSaTurama(prijem, listaTura);
            if (uspeh)
            {
                poruka = "Uspesno snimljen prijem sa turama u jednoj transakciji!";
            }
            else
            {
                poruka = "GRESKA SNIMANJA PODATAKA!";
            }
            return poruka;
        }
    }
}
