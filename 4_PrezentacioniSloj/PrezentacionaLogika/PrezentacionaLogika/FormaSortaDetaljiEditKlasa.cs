using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using System.Data;
using KlasePodataka;

namespace PrezentacionaLogika
{
    public class FormaSortaDetaljiEditKlasa
    {
   // atributi i property
        private string _stringKonekcije;
        private SPSortaDBKlasa SPSortaDBObjekat;

        private SortaKlasa preuzetaSortaObjekat;
        private SortaKlasa izmenjenaSortaObjekat;

        private string _sifraPreuzeteSorte;
        private string _nazivPreuzeteSorte;

        private string _sifraIzmenjeneSorte;
        private string _nazivIzmenjeneSorte;
        
// PROPERTY

        public string SifraPreuzeteSorte
        {
            get { return _sifraPreuzeteSorte; }
            set { _sifraPreuzeteSorte = value; _nazivPreuzeteSorte = DajNaziv(_sifraPreuzeteSorte); }
        }

        public string NazivPreuzeteSorte
        {
            get { return _nazivPreuzeteSorte; }
            // OVO NECEMO DA OMOGUCIMO, JER SE RACUNA IZ SIFRE set { pNazivPreuzeteSorte = value; }
        }

        public string SifraIzmenjeneSorte
        {
            get { return _sifraIzmenjeneSorte; }
            set { _sifraIzmenjeneSorte = value; }
        }


        public string NazivIzmenjeneSorte
        {
            get { return _nazivIzmenjeneSorte; }
            set { _nazivIzmenjeneSorte = value; }
        }


    // konstruktor
        public FormaSortaDetaljiEditKlasa(string noviStringKonekcije)
        {
            _stringKonekcije = noviStringKonekcije;
            SPSortaDBObjekat = new SPSortaDBKlasa(_stringKonekcije);
        }

        // privatne metode
        private string DajNaziv(string pomSifra)
        {
            string pomNaziv ="";
            DataSet PodaciDataSet = new DataSet();
            pomNaziv = SPSortaDBObjekat.DajNazivPremaIDSorte(pomSifra); 

            return pomNaziv;
        }

        // javne metode
        public bool ObrisiSortu()
        {
            // sorta koja je trenutno u atributima data, TJ. preuzeta sifra je bitna
            bool uspehBrisanja = false;
            uspehBrisanja = SPSortaDBObjekat.ObrisiSortu(_sifraPreuzeteSorte);  

            return uspehBrisanja;

        }

        public bool IzmeniSortu()
        {
            bool uspehIzmene = false;
            preuzetaSortaObjekat = new SortaKlasa();
            izmenjenaSortaObjekat = new SortaKlasa();

            preuzetaSortaObjekat.Sifra = _sifraPreuzeteSorte;
            preuzetaSortaObjekat.Naziv = _nazivPreuzeteSorte;

            izmenjenaSortaObjekat.Sifra = _sifraIzmenjeneSorte;
            izmenjenaSortaObjekat.Naziv = _nazivIzmenjeneSorte;

            uspehIzmene = SPSortaDBObjekat.IzmeniSortu(preuzetaSortaObjekat, izmenjenaSortaObjekat);  

            return uspehIzmene;
        }
    }
}
