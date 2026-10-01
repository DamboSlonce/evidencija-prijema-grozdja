using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using System.Data;
using KlasePodataka;

namespace PrezentacionaLogika
{
    public class FormaSortaStampaKlasa
    {
        // atributi
        private string _stringKonekcije;

        // property

        // konstruktor
        public FormaSortaStampaKlasa(string noviStringKonekcije)
        {
            _stringKonekcije = noviStringKonekcije;
        }

        // private metode

        // public metode
        public DataSet DajPodatkeZaGrid(string filter)
        {
            DataSet PodaciDataSet = new DataSet();
            SPSortaDBKlasa SPSortaDBObjekat = new SPSortaDBKlasa(_stringKonekcije);            
            if (filter.Equals(""))
            {
                PodaciDataSet = SPSortaDBObjekat.DajSveSorte(); 
            }
            else
            {
                PodaciDataSet = SPSortaDBObjekat.DajSortePoNazivu(filter); 
            }
            return PodaciDataSet;
        }
    }
}
