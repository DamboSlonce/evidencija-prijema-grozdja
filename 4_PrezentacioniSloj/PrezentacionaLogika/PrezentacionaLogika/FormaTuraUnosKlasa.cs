using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using System.Data;
using System.Globalization;
using KlasePodataka;
using PoslovnaLogika;

namespace PrezentacionaLogika
{
    public class FormaTuraUnosKlasa
    {
        // atributi
        private string _stringKonekcije;
        private string _oznaka;
        private string _brojPrijema;
        private string _datumPrijema;
        private string _isporucilac;
        private string _nazivSorte;
        private string _netoMasaKg;
        private string _secerProcenat;
        private string _kiselostGl;
        private string _temperaturaC;
        private string _udeoOstecenihProcenat;
        private string _usmerenje;

        // property
        public string Oznaka
        {
            get { return _oznaka; }
            set { _oznaka = value; }
        }
        
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

        public string NazivSorte
        {
            get { return _nazivSorte; }
            set { _nazivSorte = value; }
        }

        public string NetoMasaKg
        {
            get { return _netoMasaKg; }
            set { _netoMasaKg = value; }
        }

        public string SecerProcenat
        {
            get { return _secerProcenat; }
            set { _secerProcenat = value; }
        }

        public string KiselostGl
        {
            get { return _kiselostGl; }
            set { _kiselostGl = value; }
        }

        public string TemperaturaC
        {
            get { return _temperaturaC; }
            set { _temperaturaC = value; }
        }

        public string UdeoOstecenihProcenat
        {
            get { return _udeoOstecenihProcenat; }
            set { _udeoOstecenihProcenat = value; }
        }

        public string Usmerenje
        {
            get { return _usmerenje; }
            set { _usmerenje = value; }
        }
        
        // konstruktor
        public FormaTuraUnosKlasa(string noviStringKonekcije)
        {
            _stringKonekcije = noviStringKonekcije;
        }

        // private metode
        private decimal PretvoriUDecimal(string pomTekst)
        {
            decimal pomVrednost = 0;
            string pomNormalizovano = pomTekst.Replace(',', '.');
            pomVrednost = decimal.Parse(pomNormalizovano, CultureInfo.InvariantCulture);
            return pomVrednost;
        }

        // public metode
        public DataSet DajPodatkeZaCombo()
        {
            DataSet PodaciDataSet = new DataSet();
            SortaRepositoryKlasa sortaRepo = new SortaRepositoryKlasa(this._stringKonekcije);
            PodaciDataSet = sortaRepo.DajSveSorte();
            return PodaciDataSet;
        }

        public bool DaLiJeSvePopunjeno()
        {
            bool svePopunjeno = false;

            if (this._nazivSorte.Equals("Izaberite..."))
            {
                svePopunjeno = false;
            }
            else
            {
                if ((this._oznaka.Length > 0) && (this._brojPrijema.Length > 0) && (this._datumPrijema.Length > 0) && (this._isporucilac.Length > 0) && (this._nazivSorte.Length > 0) && (this._netoMasaKg.Length > 0) && (this._secerProcenat.Length > 0) && (this._kiselostGl.Length > 0) && (this._temperaturaC.Length > 0) && (this._udeoOstecenihProcenat.Length > 0))
                {
                    svePopunjeno = true;
                }
                else
                {
                    svePopunjeno = false;
                }
            }          

            return svePopunjeno;
        }


        public bool DaLiJeJedinstvenZapis()
        {
            bool jedinstvenZapis = false;
            DataSet PodaciDataSet = new DataSet();
            TuraRepositoryKlasa turaRepo = new TuraRepositoryKlasa(this._stringKonekcije);
            PodaciDataSet = turaRepo.DajTuruPoOznaci(this._oznaka);
            
            if (PodaciDataSet.Tables[0].Rows.Count == 0)
            {
                jedinstvenZapis = true;
            }
            else
            {
                jedinstvenZapis = false;
            }

            return jedinstvenZapis;

        }

        public bool DaLiSuPodaciUskladjeniSaPoslovnimPravilima()
        {
            // POSLOVNO PRAVILO:
            // AKO je sadrzaj secera u turi (%) manji od X za tu sortu
            // ILI je udeo ostecenih bobica veci od Y%,
            // ONDA sistem automatski usmerava turu u destilat.
            // Provera NE blokira snimanje - primenjuje AUTOMATIZAM.
            
            bool uskladjeniPodaci = true;

            decimal pomSecer = this.PretvoriUDecimal(this._secerProcenat);
            decimal pomUdeo = this.PretvoriUDecimal(this._udeoOstecenihProcenat);

            UsmerenjePreradeKlasa UsmerenjePreradeObjekat = new UsmerenjePreradeKlasa(this._stringKonekcije);
            string pomOdredjenoUsmerenje = UsmerenjePreradeObjekat.OdrediUsmerenje(this._nazivSorte, pomSecer, pomUdeo);
            this._usmerenje = pomOdredjenoUsmerenje;

            return uskladjeniPodaci;
        }

        public string SnimiPodatke()
        {
            string porukaUspehaSnimanja="";
            bool uspehSnimanja = false;
            string izabranoUsmerenje = this._usmerenje;

            // 1. provera popunjenosti
            bool SvePopunjeno = this.DaLiJeSvePopunjeno();
            if (SvePopunjeno==true) {
                porukaUspehaSnimanja = "Sve je popunjeno!";
             }
            else
            {
                porukaUspehaSnimanja = "NIJE SVE POPUNJENO!";
                return porukaUspehaSnimanja;
            }

            // 2. provera ispravnosti - karakteri, vrednost iz domena, jedinstvenost zapisa
            bool JedinstvenZapis = this.DaLiJeJedinstvenZapis();
            if (JedinstvenZapis==true)
            {
                porukaUspehaSnimanja = porukaUspehaSnimanja + "Jeste jedinstven zapis!";
            }
            else
            {
                porukaUspehaSnimanja = porukaUspehaSnimanja + "NIJE JEDINSTVEN ZAPIS!";
                return porukaUspehaSnimanja;
            }

            // 3. provera ispravnosti - provera uskladjenosti podataka sa poslovnim pravilima
            // AUTOMATIZAM: uvek se odredjuje usmerenje, snimanje se NE blokira
            bool UskladjenoSaPoslovnimPravilima = this.DaLiSuPodaciUskladjeniSaPoslovnimPravilima();

            if (UskladjenoSaPoslovnimPravilima==true)
             {
                porukaUspehaSnimanja = porukaUspehaSnimanja + "Jeste uskladjeno sa poslovnim pravilima!";
            }
            else
            {
               porukaUspehaSnimanja = porukaUspehaSnimanja + "PODACI NISU USKLADJENI SA POSLOVNIM PRAVILIMA!";
               return porukaUspehaSnimanja;
            }
            
            if ((SvePopunjeno==true) && (JedinstvenZapis==true) && (UskladjenoSaPoslovnimPravilima==true)){
                TuraKlasa TuraObjekat = new TuraKlasa();
                TuraObjekat.Oznaka = _oznaka;
                TuraObjekat.BrojPrijema = _brojPrijema;
                TuraObjekat.DatumPrijema = DateTime.Parse(_datumPrijema);
                TuraObjekat.Isporucilac = _isporucilac;
                TuraObjekat.NetoMasaKg = this.PretvoriUDecimal(_netoMasaKg);
                TuraObjekat.SecerProcenat = this.PretvoriUDecimal(_secerProcenat);
                TuraObjekat.KiselostGl = this.PretvoriUDecimal(_kiselostGl);
                TuraObjekat.TemperaturaC = this.PretvoriUDecimal(_temperaturaC);
                TuraObjekat.UdeoOstecenihProcenat = this.PretvoriUDecimal(_udeoOstecenihProcenat);

                UsmerenjePreradeKlasa UsmerenjePreradeObjekat = new UsmerenjePreradeKlasa(_stringKonekcije);
                string pomUsmerenje = UsmerenjePreradeObjekat.OdrediUsmerenje(_nazivSorte, TuraObjekat.SecerProcenat, TuraObjekat.UdeoOstecenihProcenat);
                TuraObjekat.Usmerenje = pomUsmerenje;
                this._usmerenje = pomUsmerenje;

                SortaKlasa SortaObjekat = new SortaKlasa();
                SortaRepositoryKlasa sortaRepo = new SortaRepositoryKlasa(_stringKonekcije);
                SortaObjekat.Sifra = sortaRepo.DajSifruSortePoNazivu(_nazivSorte);
                SortaObjekat.Naziv = _nazivSorte;
                TuraObjekat.Sorta = SortaObjekat;

                TuraRepositoryKlasa turaRepo = new TuraRepositoryKlasa(_stringKonekcije);
                uspehSnimanja = turaRepo.SnimiNovuTuru(TuraObjekat);
                if (uspehSnimanja)
                {
                    porukaUspehaSnimanja = porukaUspehaSnimanja +  "Uspesno snimljeni novi podaci!";
                    if (pomUsmerenje == "destilat")
                    {
                        porukaUspehaSnimanja = porukaUspehaSnimanja + " AUTOMATIZAM: tura je usmerena u destilat (secer ispod minimuma za sortu ili udeo ostecenih bobica veci od dozvoljenog).";
                        if ((izabranoUsmerenje == "kvalitetno vino"))
                        {
                            porukaUspehaSnimanja = porukaUspehaSnimanja + " Izbor 'kvalitetno vino' je preusmeren u destilat.";
                        }
                    }
                    else
                    {
                        porukaUspehaSnimanja = porukaUspehaSnimanja + " Usmerenje: kvalitetno vino.";
                    }
                }
                else
                {
                    porukaUspehaSnimanja = porukaUspehaSnimanja + "GRESKA SNIMANJA PODATAKA!";
                }
                return porukaUspehaSnimanja;
            }
            else
            {
                return "NEUSPEH SNIMANJA!";
            }
            
        }

        
    }
}
