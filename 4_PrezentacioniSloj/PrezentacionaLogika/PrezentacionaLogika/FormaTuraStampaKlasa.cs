using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using System.Data;
using KlasePodataka;

namespace PrezentacionaLogika
{
    public class FormaTuraStampaKlasa
    {
        // atributi
        private string _stringKonekcije;

        // property

        // konstruktor
        public FormaTuraStampaKlasa(string noviStringKonekcije)
        {
            _stringKonekcije = noviStringKonekcije;
        }

        // private metode

        // public metode
        public DataSet DajPodatkeZaGrid(string filter)
        {
            DataSet PodaciDataSet = new DataSet();
            SPTuraDBKlasa SPTuraDBObjekat = new SPTuraDBKlasa(_stringKonekcije);
            if (filter.Equals(""))
            {
                PodaciDataSet = SPTuraDBObjekat.DajSveTure();
            }
            else
            {
                PodaciDataSet = SPTuraDBObjekat.DajTuruPoIsporuciocu(filter);
            }
            return PodaciDataSet;
        }
    }
}
