using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PoslovnaLogika
{
    public class ParametriKvalitetaKlasa
    {
        public decimal DajMinSecerZaSortu(String sifraSorteWSParametar)
        {
            WSParametriKvaliteta.OgranicenjaKvaliteta objOgranicenja = new WSParametriKvaliteta.OgranicenjaKvaliteta();
            decimal pomMinSecer = objOgranicenja.DajMinSecer(sifraSorteWSParametar);
            return pomMinSecer;
        }

        public decimal DajMaxUdeoOstecenihZaSortu(String sifraSorteWSParametar)
        {
            WSParametriKvaliteta.OgranicenjaKvaliteta objOgranicenja = new WSParametriKvaliteta.OgranicenjaKvaliteta();
            decimal pomMaxUdeo = objOgranicenja.DajMaxUdeoOstecenih(sifraSorteWSParametar);
            return pomMaxUdeo;
        }

    }
}
