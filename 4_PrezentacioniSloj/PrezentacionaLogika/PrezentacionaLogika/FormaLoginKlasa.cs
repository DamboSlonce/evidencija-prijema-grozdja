using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using KlasePodataka;
using System.Data;

namespace PrezentacionaLogika
{
    public class FormaLoginKlasa
    {

        private string _stringKonekcije;
        private string _korisnickoIme;
        private string _sifra;


        public string KorisnickoIme
        {
            get { return _korisnickoIme; }
            set { _korisnickoIme = value; }
        }
        

        public string Sifra
        {
            get { return _sifra; }
            set { _sifra = value; }
        }

        // konstruktor
        public FormaLoginKlasa(string NoviStringKonekcije)
        {
            _stringKonekcije = NoviStringKonekcije;
        }


        // javne metode
        public bool VazeciKorisnik()
        {
            bool vazeci = false;

            SPKorisnikDBKlasa SPKorisnikDBObjekat = new SPKorisnikDBKlasa(_stringKonekcije);
            DataSet PodaciDataSet = SPKorisnikDBObjekat.DajKorisnikaPoKorisnickomImenuISifri(_korisnickoIme, _sifra);

            if (PodaciDataSet.Tables[0].Rows.Count > 0)
                // pronasao ga je u bazi
            {
                vazeci = true;
            }
            else
            {
                vazeci = false;
            }

            return vazeci;

        }

        public string DajImePrezimeKorisnika()
        {
            string imePrezime = "";

            SPKorisnikDBKlasa SPKorisnikDBObjekat = new SPKorisnikDBKlasa(_stringKonekcije);
            DataSet PodaciDataSet = SPKorisnikDBObjekat.DajKorisnikaPoKorisnickomImenuISifri(_korisnickoIme, _sifra);

            if (PodaciDataSet.Tables[0].Rows.Count > 0)
            // pronasao ga je u bazi
            {
                imePrezime = PodaciDataSet.Tables[0].Rows[0].ItemArray[2].ToString() + " " + PodaciDataSet.Tables[0].Rows[0].ItemArray[1].ToString();
            }
            return imePrezime;

        }




    }
}
