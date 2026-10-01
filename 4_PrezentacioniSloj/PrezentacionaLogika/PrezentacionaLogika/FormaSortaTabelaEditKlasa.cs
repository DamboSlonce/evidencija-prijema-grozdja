using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using System.Data;
using KlasePodataka;

namespace PrezentacionaLogika
{
    public class FormaSortaTabelaEditKlasa
    {
        // atributi
        private string _stringKonekcije;

        // property

        // konstruktor
        public FormaSortaTabelaEditKlasa(string noviStringKonekcije)
        {
            _stringKonekcije = noviStringKonekcije;
        }

        // private metode

        // public metode
        public DataSet DajPodatkeZaGrid(string filter)
        {
            DataSet PodaciDataSet = new DataSet();
            if (filter.Equals(""))
            {
                // Treci nacin rada sa bazom: Entity Framework (nasledjivanje DbContext)
                SortaRepositoryKlasa sortaRepo = new SortaRepositoryKlasa(_stringKonekcije);
                PodaciDataSet = sortaRepo.DajSveSortePrekoEntityFramework();
            }
            else
            {
                SPSortaDBKlasa SPSortaDBObjekat = new SPSortaDBKlasa(_stringKonekcije);
                PodaciDataSet = SPSortaDBObjekat.DajSortePoNazivu(filter);
            }
            return PodaciDataSet;
        }
    }
}
