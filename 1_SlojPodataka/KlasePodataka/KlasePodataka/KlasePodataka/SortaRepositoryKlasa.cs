using System.Data;

namespace KlasePodataka
{
    public class SortaRepositoryKlasa
    {
        private string _stringKonekcije;

        public SortaRepositoryKlasa(string noviStringKonekcije)
        {
            _stringKonekcije = noviStringKonekcije;
        }

        public DataSet DajSveSorte()
        {
            SPSortaDBKlasa spObjekat = new SPSortaDBKlasa(_stringKonekcije);
            return spObjekat.DajSveSorte();
        }

        public string DajSifruSortePoNazivu(string nazivSorte)
        {
            SPSortaDBKlasa spObjekat = new SPSortaDBKlasa(_stringKonekcije);
            return spObjekat.DajSifruSortePoNazivu(nazivSorte);
        }

        public bool SnimiNovuSortu(SortaKlasa novaSortaObjekat)
        {
            SPSortaDBKlasa spObjekat = new SPSortaDBKlasa(_stringKonekcije);
            return spObjekat.SnimiNovuSortu(novaSortaObjekat);
        }

        public DataSet DajSveSortePrekoEntityFramework()
        {
            SortaEfRepositoryKlasa efRepo = new SortaEfRepositoryKlasa(this._stringKonekcije);
            return efRepo.DajSveKaoDataSet();
        }
    }
}
