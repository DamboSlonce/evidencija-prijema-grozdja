using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using KlasePodataka;

namespace PrezentacionaLogika
{
    public class FormaSortaUnosKlasa
    {
        // atributi
        private string _stringKonekcije;
        private string _sifra;
        private string _naziv;
        
        // property
        public string Sifra
        {
            get { return _sifra; }
            set { _sifra = value; }
        }

        public string Naziv
        {
            get { return _naziv; }
            set { _naziv = value; }
        }

        public FormaSortaUnosKlasa(string noviStringKonekcije)
        {
            _stringKonekcije = noviStringKonekcije;
        }

        public bool SnimiPodatke()
        {
            bool uspehSnimanja = false;

            SortaRepositoryKlasa sortaRepo = new SortaRepositoryKlasa(this._stringKonekcije);
            SortaKlasa SortaObjekat = new SortaKlasa();
            SortaObjekat.Sifra = this._sifra;
            SortaObjekat.Naziv = this._naziv;
            uspehSnimanja = sortaRepo.SnimiNovuSortu(SortaObjekat);

            return uspehSnimanja;

        }



        }
}
