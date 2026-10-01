using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using DBUtils;

namespace KlasePodataka
{
    public class PrijemDBKlasa : TabelaKlasa
    {
        public PrijemDBKlasa(KonekcijaKlasa novaKonekcija, string noviNazivTabele)
            : base(novaKonekcija, noviNazivTabele)
        {
        }

        public DataSet DajSvePrijeme()
        {
            return this.DajPodatke("select * from PRIJEM");
        }

        public DataSet DajPrijemPoBroju(string brojPrijema)
        {
            string pomBroj = Esc(brojPrijema);
            return this.DajPodatke("select * from PRIJEM where BrojPrijema=N'" + pomBroj + "'");
        }

        public DataSet DajTurePoBrojuPrijema(string brojPrijema)
        {
            string pomBroj = Esc(brojPrijema);
            string pomUpit = "Select Tura.Oznaka, Tura.BrojPrijema, Tura.DatumPrijema, Tura.Isporucilac, Sorta.Naziv as NazivSorte, " +
                "Tura.NetoMasaKg, Tura.SecerProcenat, Tura.KiselostGl, Tura.TemperaturaC, Tura.UdeoOstecenihProcenat, Tura.Usmerenje " +
                "from Tura inner join Sorta on Tura.SifraSorte = Sorta.Sifra where Tura.BrojPrijema=N'" + pomBroj + "'";
            return this.DajPodatke(pomUpit);
        }

        public bool SnimiPrijemSaTurama(PrijemKlasa prijemObjekat, List<TuraKlasa> listaTura)
        {
            List<string> listaUpita = new List<string>();
            string pomBroj = Esc(prijemObjekat.BrojPrijema);
            string pomDatum = prijemObjekat.DatumPrijema.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string pomIsporucilac = Esc(prijemObjekat.Isporucilac);

            listaUpita.Add("IF NOT EXISTS (SELECT 1 FROM PRIJEM WHERE BrojPrijema=N'" + pomBroj + "') " +
                "INSERT INTO PRIJEM (BrojPrijema, DatumPrijema, Isporucilac) VALUES (N'" + pomBroj + "', '" + pomDatum + "', N'" + pomIsporucilac + "')");

            for (int i = 0; i < listaTura.Count; i++)
            {
                TuraKlasa tura = listaTura[i];
                listaUpita.Add(NapraviInsertTure(tura));
            }

            return this.IzvrsiAzuriranje(listaUpita);
        }

        private string NapraviInsertTure(TuraKlasa tura)
        {
            string pomDatum = tura.DatumPrijema.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string pomNeto = tura.NetoMasaKg.ToString(CultureInfo.InvariantCulture);
            string pomSecer = tura.SecerProcenat.ToString(CultureInfo.InvariantCulture);
            string pomKiselost = tura.KiselostGl.ToString(CultureInfo.InvariantCulture);
            string pomTemperatura = tura.TemperaturaC.ToString(CultureInfo.InvariantCulture);
            string pomUdeo = tura.UdeoOstecenihProcenat.ToString(CultureInfo.InvariantCulture);
            return "insert into Tura values(N'" + Esc(tura.Oznaka) + "', N'" + Esc(tura.BrojPrijema) + "','" + pomDatum +
                "',N'" + Esc(tura.Isporucilac) + "',N'" + Esc(tura.Sorta.Sifra) + "'," + pomNeto + "," + pomSecer + "," +
                pomKiselost + "," + pomTemperatura + "," + pomUdeo + ",N'" + Esc(tura.Usmerenje) + "')";
        }

        private string Esc(string tekst)
        {
            if (tekst == null)
            {
                return "";
            }
            return tekst.Replace("'", "''");
        }
    }
}
