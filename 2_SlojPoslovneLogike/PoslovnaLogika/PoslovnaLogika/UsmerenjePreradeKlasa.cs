using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using KlasePodataka;
using KlaseMapiranja;

namespace PoslovnaLogika
{
    public class UsmerenjePreradeKlasa
    {
        // TREBALO BI DA SE PRERADI IZ 3 KLASE:
        // Parametri kvaliteta - izdvaja ogranicenje iz SOAP/XML
        // AnalizaTure - merenja ture (secer, udeo ostecenih)
        // UsmerenjePrerade - uporedjuje merenja sa ogranicenjima i odredjuje destinaciju
        
        // atributi
        private string _stringKonekcije;

        // property

        // konstruktor
        public UsmerenjePreradeKlasa(string NoviStringKonekcije)
        {
            _stringKonekcije = NoviStringKonekcije;
        }

        // privatne metode

        // public metode

        public string OdrediUsmerenje(string nazivSorte, decimal secer, decimal udeoOstecenih)
        {
            // POSLOVNO PRAVILO:
            // AKO je sadrzaj secera u turi (%) manji od X za tu sortu
            // ILI je udeo ostecenih bobica veci od Y%,
            // ONDA sistem automatski usmerava turu u destilat
            // i ne dozvoljava usmerenje "kvalitetno vino".

            string pomUsmerenje = "kvalitetno vino";

            AnalizaTureKlasa analizaObjekat = new AnalizaTureKlasa(secer, udeoOstecenih);

            // ################################################################
            // 1. MAPIRANJE SLOJEVA - uskladjivanje sifre sorte iz raznih delova programa
            // Web servis ima R - Rizling, baza podataka ima R - Rizling (heuristika: prvo slovo naziva)
            string pomIdSorteWS = "";
            MaperKlasa maperObjekat = new MaperKlasa(_stringKonekcije);
            pomIdSorteWS = maperObjekat.DajSifruSorteZaWebServis(nazivSorte);

            // ################################################################
            // 2. IZDVAJANJE MIN SECERA I MAX UDELA OSTECENIH ZA ODG SORTU
            ParametriKvalitetaKlasa parametriObjekat = new ParametriKvalitetaKlasa();
            decimal pomMinSecer = parametriObjekat.DajMinSecerZaSortu(pomIdSorteWS);
            decimal pomMaxUdeoOstecenih = parametriObjekat.DajMaxUdeoOstecenihZaSortu(pomIdSorteWS);

            // ################################################################
            // 3. UPOREDJIVANJE MERENJA I OGRANICENJA
            if ((analizaObjekat.SecerProcenat < pomMinSecer) || (analizaObjekat.UdeoOstecenihProcenat > pomMaxUdeoOstecenih))
            {
                pomUsmerenje = "destilat";
            }
            else
            {
                pomUsmerenje = "kvalitetno vino";
            }
            return pomUsmerenje;
        }

        public bool DaLiMozeKvalitetnoVino(string nazivSorte, decimal secer, decimal udeoOstecenih)
        {
            bool mozeKvalitetnoVino = false;
            string pomUsmerenje = this.OdrediUsmerenje(nazivSorte, secer, udeoOstecenih);
            if (pomUsmerenje == "kvalitetno vino")
            {
                mozeKvalitetnoVino = true;
            }
            else
            {
                mozeKvalitetnoVino = false;
            }
            return mozeKvalitetnoVino;
        }
    }
}
