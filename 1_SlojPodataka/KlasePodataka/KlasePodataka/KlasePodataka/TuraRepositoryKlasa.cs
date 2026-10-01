using System.Data;
using DBUtils;

namespace KlasePodataka
{
    public class TuraRepositoryKlasa
    {
        private string _stringKonekcije;

        public TuraRepositoryKlasa(string noviStringKonekcije)
        {
            _stringKonekcije = noviStringKonekcije;
        }

        public DataSet DajSveTure()
        {
            SPTuraDBKlasa spObjekat = new SPTuraDBKlasa(_stringKonekcije);
            return spObjekat.DajSveTure();
        }

        public DataSet DajTuruPoIsporuciocu(string isporucilacFilter)
        {
            SPTuraDBKlasa spObjekat = new SPTuraDBKlasa(_stringKonekcije);
            return spObjekat.DajTuruPoIsporuciocu(isporucilacFilter);
        }

        public DataSet DajTuruPoOznaci(string oznakaFilter)
        {
            SPTuraDBKlasa spObjekat = new SPTuraDBKlasa(_stringKonekcije);
            return spObjekat.DajTuruPoOznaci(oznakaFilter);
        }

        public bool SnimiNovuTuru(TuraKlasa novaTuraObjekat)
        {
            PrijemRepositoryKlasa prijemRepo = new PrijemRepositoryKlasa(_stringKonekcije);
            return prijemRepo.SnimiJednuTuruSaZaglavljem(novaTuraObjekat);
        }

        public bool ObrisiTuru(string oznaka)
        {
            SPTuraDBKlasa spObjekat = new SPTuraDBKlasa(_stringKonekcije);
            return spObjekat.ObrisiTuru(oznaka);
        }

        public DataSet DajSveTurePrekoNasledjeneKlase()
        {
            KonekcijaKlasa konekcija = new KonekcijaKlasa(_stringKonekcije);
            konekcija.OtvoriKonekciju();
            try
            {
                TuraDBKlasa turaDB = new TuraDBKlasa(konekcija, "TURA");
                return turaDB.DajSveTure();
            }
            finally
            {
                konekcija.ZatvoriKonekciju();
            }
        }
    }
}
