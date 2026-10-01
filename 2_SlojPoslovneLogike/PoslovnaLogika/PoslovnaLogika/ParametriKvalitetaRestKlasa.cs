using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;

namespace PoslovnaLogika
{
    public class ParametriKvalitetaRestKlasa
    {
        private string _osnovniUrl;

        public ParametriKvalitetaRestKlasa()
        {
            _osnovniUrl = "http://localhost:1718/OgranicenjaRest.ashx";
        }

        public decimal DajMinSecerZaSortu(string sifraSorte)
        {
            string json = this.PozoviRest("DajMinSecer", sifraSorte);
            return this.IzvuciDecimal(json, "minSecer");
        }

        public decimal DajMaxUdeoOstecenihZaSortu(string sifraSorte)
        {
            string json = this.PozoviRest("DajMaxUdeoOstecenih", sifraSorte);
            return this.IzvuciDecimal(json, "maxUdeoOstecenih");
        }

        private string PozoviRest(string akcija, string sifraSorte)
        {
            string url = _osnovniUrl + "?akcija=" + akcija + "&sifra=" + sifraSorte;
            WebClient klijent = new WebClient();
            klijent.Encoding = Encoding.UTF8;
            return klijent.DownloadString(url);
        }

        private decimal IzvuciDecimal(string json, string imePolja)
        {
            string kljuc = "\"" + imePolja + "\":";
            int i = json.IndexOf(kljuc);
            if (i < 0)
            {
                return 0;
            }
            int start = i + kljuc.Length;
            StringBuilder broj = new StringBuilder();
            for (int k = start; k < json.Length; k++)
            {
                char c = json[k];
                if ((c == '-') || (c == '.') || (c >= '0' && c <= '9'))
                {
                    broj.Append(c);
                }
                else
                {
                    if (broj.Length > 0)
                    {
                        break;
                    }
                }
            }
            return decimal.Parse(broj.ToString(), CultureInfo.InvariantCulture);
        }
    }
}
