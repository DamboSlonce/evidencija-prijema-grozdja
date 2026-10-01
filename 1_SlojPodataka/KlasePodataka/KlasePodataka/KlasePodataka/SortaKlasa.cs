using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KlasePodataka
{
    public class SortaKlasa
    {
        // atributi
        private string _sifra;
        private string _naziv;

        // property
        public string Sifra
        {
            get
            {
                return _sifra;
            }
            set
            {
                if (this._sifra != value)
                    this._sifra = value;
            }
        }

        public string Naziv
        {
            get
            {
                return _naziv;
            }
            set
            {
                if (this._naziv != value)
                    this._naziv = value;
            }
        }


        // konstruktor
        public SortaKlasa()
        {
            _sifra = "";
            _naziv = "";
        }

        
    }
}
