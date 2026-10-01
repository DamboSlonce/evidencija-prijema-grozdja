using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using KlasePodataka;

namespace PoslovnaLogika
{
    public class AnalizaTureKlasa
    {
        // merenja ture - NE broj zapisa, vec parametri kvaliteta grozdja
        private decimal _secerProcenat;
        private decimal _udeoOstecenihProcenat;

        public decimal SecerProcenat
        {
            get { return _secerProcenat; }
            set { _secerProcenat = value; }
        }

        public decimal UdeoOstecenihProcenat
        {
            get { return _udeoOstecenihProcenat; }
            set { _udeoOstecenihProcenat = value; }
        }

        public AnalizaTureKlasa()
        {
            _secerProcenat = 0;
            _udeoOstecenihProcenat = 0;
        }

        public AnalizaTureKlasa(decimal noviSecerProcenat, decimal noviUdeoOstecenihProcenat)
        {
            _secerProcenat = noviSecerProcenat;
            _udeoOstecenihProcenat = noviUdeoOstecenihProcenat;
        }

    }
}
