using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DBUtils;
using System.Data;
using System.Globalization;

namespace KlasePodataka
{
    public class TuraDBKlasa: TabelaKlasa
    {
        

        public TuraDBKlasa(KonekcijaKlasa novaKonekcija, string noviNazivTabele):base(novaKonekcija, noviNazivTabele)
        {
            // nesto drugo u vezi specificno ove klase
        }
        
        
        public DataSet DajSveTure()
        {
            return this.DajPodatke("select * from Tura");
        }

        public bool DodajNovuTuru(TuraKlasa novaTuraObjekat)
        {
            string pomDatum = novaTuraObjekat.DatumPrijema.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string pomNeto = novaTuraObjekat.NetoMasaKg.ToString(CultureInfo.InvariantCulture);
            string pomSecer = novaTuraObjekat.SecerProcenat.ToString(CultureInfo.InvariantCulture);
            string pomKiselost = novaTuraObjekat.KiselostGl.ToString(CultureInfo.InvariantCulture);
            string pomTemperatura = novaTuraObjekat.TemperaturaC.ToString(CultureInfo.InvariantCulture);
            string pomUdeo = novaTuraObjekat.UdeoOstecenihProcenat.ToString(CultureInfo.InvariantCulture);
            string upit="insert into Tura values('" + novaTuraObjekat.Oznaka + "', '" + novaTuraObjekat.BrojPrijema + "','" + pomDatum + "','" + novaTuraObjekat.Isporucilac + "','" + novaTuraObjekat.Sorta.Sifra + "'," + pomNeto + "," + pomSecer + "," + pomKiselost + "," + pomTemperatura + "," + pomUdeo + ",'" + novaTuraObjekat.Usmerenje + "')";
            return this.IzvrsiAzuriranje(upit);

        }
    }
}
