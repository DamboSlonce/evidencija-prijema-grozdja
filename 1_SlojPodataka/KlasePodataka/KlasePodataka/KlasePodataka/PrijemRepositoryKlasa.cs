using System.Collections.Generic;
using System.Data;
using DBUtils;

namespace KlasePodataka
{
    public class PrijemRepositoryKlasa
    {
        private string _stringKonekcije;

        public PrijemRepositoryKlasa(string noviStringKonekcije)
        {
            _stringKonekcije = noviStringKonekcije;
        }

        public DataSet DajSvePrijeme()
        {
            KonekcijaKlasa konekcija = new KonekcijaKlasa(_stringKonekcije);
            konekcija.OtvoriKonekciju();
            try
            {
                PrijemDBKlasa prijemDB = new PrijemDBKlasa(konekcija, "PRIJEM");
                return prijemDB.DajSvePrijeme();
            }
            finally
            {
                konekcija.ZatvoriKonekciju();
            }
        }

        public DataSet DajPrijemPoBroju(string brojPrijema)
        {
            KonekcijaKlasa konekcija = new KonekcijaKlasa(_stringKonekcije);
            konekcija.OtvoriKonekciju();
            try
            {
                PrijemDBKlasa prijemDB = new PrijemDBKlasa(konekcija, "PRIJEM");
                return prijemDB.DajPrijemPoBroju(brojPrijema);
            }
            finally
            {
                konekcija.ZatvoriKonekciju();
            }
        }

        public DataSet DajTurePoBrojuPrijema(string brojPrijema)
        {
            KonekcijaKlasa konekcija = new KonekcijaKlasa(_stringKonekcije);
            konekcija.OtvoriKonekciju();
            try
            {
                PrijemDBKlasa prijemDB = new PrijemDBKlasa(konekcija, "PRIJEM");
                return prijemDB.DajTurePoBrojuPrijema(brojPrijema);
            }
            finally
            {
                konekcija.ZatvoriKonekciju();
            }
        }

        public bool SnimiPrijemSaTurama(PrijemKlasa prijemObjekat, List<TuraKlasa> listaTura)
        {
            KonekcijaKlasa konekcija = new KonekcijaKlasa(_stringKonekcije);
            konekcija.OtvoriKonekciju();
            try
            {
                PrijemDBKlasa prijemDB = new PrijemDBKlasa(konekcija, "PRIJEM");
                return prijemDB.SnimiPrijemSaTurama(prijemObjekat, listaTura);
            }
            finally
            {
                konekcija.ZatvoriKonekciju();
            }
        }

        public bool SnimiJednuTuruSaZaglavljem(TuraKlasa turaObjekat)
        {
            PrijemKlasa prijem = new PrijemKlasa();
            prijem.BrojPrijema = turaObjekat.BrojPrijema;
            prijem.DatumPrijema = turaObjekat.DatumPrijema;
            prijem.Isporucilac = turaObjekat.Isporucilac;
            List<TuraKlasa> lista = new List<TuraKlasa>();
            lista.Add(turaObjekat);
            return this.SnimiPrijemSaTurama(prijem, lista);
        }
    }
}
