using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using System.Data;
using KlasePodataka;

namespace PrezentacionaLogika
{
    public class FormaTuraSpisakKlasa
    {
        // atributi
        private string _stringKonekcije;

        // property

        // konstruktor
        public FormaTuraSpisakKlasa(string noviStringKonekcije)
        {
            _stringKonekcije = noviStringKonekcije;
        }

        // private metode

        // public metode
        public DataSet DajPodatkeZaGrid(string filter)
        {
            DataSet PodaciDataSet = new DataSet();
            TuraRepositoryKlasa turaRepo = new TuraRepositoryKlasa(_stringKonekcije);
            if (filter.Equals(""))
            {
                PodaciDataSet = turaRepo.DajSveTure();
            }
            else
            {
                PodaciDataSet = turaRepo.DajTuruPoIsporuciocu(filter);
            }
            return PodaciDataSet;
        }

    }
}
