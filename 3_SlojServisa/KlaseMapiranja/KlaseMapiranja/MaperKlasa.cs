using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using KlasePodataka;

namespace KlaseMapiranja
{
    public class MaperKlasa
    {
        // atributi
        private string pStringKonekcije;

        // property

        // konstruktor
        public MaperKlasa(string noviStringKonekcije)
        {
            pStringKonekcije = noviStringKonekcije;
        }
        public string DajSifruSorteZaWebServis(string NazivSorteIzBazePodatakaParametar)
        {
            string pomIDSorteWS = "";
            
            // OVO JE HEURISTIKA:
            // prvo slovo naziva je to sifra u drugom sistemu
            // DAKLE: sifra sorte za web servis je prvo slovo naziva sorte, znaci za Rizling je "R"
            pomIDSorteWS = NazivSorteIzBazePodatakaParametar[0].ToString();

            return pomIDSorteWS;

        }

    }
}
