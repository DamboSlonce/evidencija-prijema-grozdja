using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using System.Data;
using System.Data.SqlClient;

namespace KlasePodataka
{
    public class SPTuraDBKlasa
    {
        // atributi
        private string _stringKonekcije;

        // property
        // 1. nacin
        public string StringKonekcije
        {
            get
            {
                return _stringKonekcije;
            }
        }
        // konstruktor
        // 2. nacin prijema vrednosti stringa konekcije spolja i dodele atributu
        public SPTuraDBKlasa(string noviStringKonekcije)
        // OVO JE DOBRO JER OBAVEZUJE DA SE PRILIKOM INSTANCIRANJA OVE KLASE
        // MORA OBEZBEDITI STRING KONEKCIJE
        {
            _stringKonekcije = noviStringKonekcije; 
        }

        // privatne metode

        // javne metode
        public DataSet DajSveTure()
        {
            // MOGU biti jos neke procedure, mogu SE VRATITI VREDNOSTI I U LISTU, DATA TABLE...
            DataSet PodaciDataSet = new DataSet();

            SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
            pomKonekcija.Open();
            SqlCommand pomKomanda = new SqlCommand("DajSveTureSaJoin", pomKonekcija);
            pomKomanda.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = pomKomanda;
            adapter.Fill(PodaciDataSet);
            pomKonekcija.Close();
            pomKonekcija.Dispose();
            
            return PodaciDataSet;
        }

        public DataSet DajTuruPoIsporuciocu(string isporucilacFilter)
        {
            // MOGU biti jos neke procedure, mogu SE VRATITI VREDNOSTI I U LISTU, DATA TABLE...
            DataSet PodaciDataSet = new DataSet();

            SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
            pomKonekcija.Open();
            SqlCommand pomKomanda = new SqlCommand("DajTuruPoIsporuciocu", pomKonekcija);
            pomKomanda.CommandType = CommandType.StoredProcedure;
            pomKomanda.Parameters.Add("@Isporucilac", SqlDbType.NVarChar).Value = isporucilacFilter;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = pomKomanda;
            adapter.Fill(PodaciDataSet);
            pomKonekcija.Close();
            pomKonekcija.Dispose();

            return PodaciDataSet;
        }


        public DataSet DajTuruPoOznaci(string oznakaFilter)
        {
            // MOGU biti jos neke procedure, mogu SE VRATITI VREDNOSTI I U LISTU, DATA TABLE...
            DataSet PodaciDataSet = new DataSet();

            SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
            pomKonekcija.Open();
            SqlCommand pomKomanda = new SqlCommand("DajTuruPoOznaci", pomKonekcija);
            pomKomanda.CommandType = CommandType.StoredProcedure;
            pomKomanda.Parameters.Add("@OznakaTure", SqlDbType.NVarChar).Value = oznakaFilter;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = pomKomanda;
            adapter.Fill(PodaciDataSet);
            pomKonekcija.Close();
            pomKonekcija.Dispose();

            return PodaciDataSet;
        }

        public int DajUkupnoTuraZaSortu(string sifraSorteFilter)
        {
            int ukupnoTura=0;
            DataSet PodaciDataSet = new DataSet();
            SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
            pomKonekcija.Open();
            SqlCommand pomKomanda = new SqlCommand("DajBrojTuraPremaSifriSorte", pomKonekcija);
            pomKomanda.CommandType = CommandType.StoredProcedure;
            pomKomanda.Parameters.Add("@SifraSorte", SqlDbType.NVarChar).Value = sifraSorteFilter;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = pomKomanda;
            adapter.Fill(PodaciDataSet);
            pomKonekcija.Close();
            pomKonekcija.Dispose();
            ukupnoTura = int.Parse(PodaciDataSet.Tables[0].Rows[0].ItemArray[0].ToString());  
            return ukupnoTura;
        } 


        private TuraListaKlasa DajListuSvihTura()
        {
            // PRIPREMA PROMENLJIVIH
            TuraListaKlasa turaListaObjekat = new TuraListaKlasa();
            DataSet podaciDataSetTura = new DataSet();
            TuraKlasa TuraObjekat;
            SortaKlasa SortaObjekat;

            SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
            pomKonekcija.Open();
            SqlCommand pomKomanda = new SqlCommand("DajSveTureSaJoinSifromSorte", pomKonekcija);
            pomKomanda.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = pomKomanda;
            adapter.Fill(podaciDataSetTura);
            pomKonekcija.Close();
            pomKonekcija.Dispose();

            // FORMIRANJE OBJEKATA I UBACIVANJE U LISTU
            for (int brojac = 0; brojac < podaciDataSetTura.Tables[0].Rows.Count; brojac++)
            {
                SortaObjekat = new SortaKlasa();
                SortaObjekat.Sifra = podaciDataSetTura.Tables[0].Rows[brojac]["SifraSorte"].ToString();
                SortaObjekat.Naziv = podaciDataSetTura.Tables[0].Rows[brojac]["NazivSorte"].ToString();

                TuraObjekat = new TuraKlasa();
                TuraObjekat.Oznaka = podaciDataSetTura.Tables[0].Rows[brojac]["Oznaka"].ToString(); 
                TuraObjekat.BrojPrijema = podaciDataSetTura.Tables[0].Rows[brojac]["BrojPrijema"].ToString(); 
                TuraObjekat.Isporucilac = podaciDataSetTura.Tables[0].Rows[brojac]["Isporucilac"].ToString();
                TuraObjekat.Sorta = SortaObjekat;
                turaListaObjekat.DodajElementListe (TuraObjekat);
            }

            return turaListaObjekat;
        }
        

        public bool SnimiNovuTuru(TuraKlasa novaTuraObjekat)
        {
            // LOKALNE PROMENLJIVE UVEK NA VRHU
            int brojSlogova =0;

            SqlConnection pompomKonekcija = new SqlConnection(_stringKonekcije);
            pompomKonekcija.Open();

            SqlCommand pompomKomanda = new SqlCommand("DodajNovuTuru", pompomKonekcija);
            pompomKomanda.CommandType = CommandType.StoredProcedure;
            pompomKomanda.Parameters.Add("@Oznaka", SqlDbType.NVarChar).Value = novaTuraObjekat.Oznaka;
            pompomKomanda.Parameters.Add("@BrojPrijema", SqlDbType.NVarChar).Value = novaTuraObjekat.BrojPrijema;
            pompomKomanda.Parameters.Add("@DatumPrijema", SqlDbType.Date).Value = novaTuraObjekat.DatumPrijema;
            pompomKomanda.Parameters.Add("@Isporucilac", SqlDbType.NVarChar).Value = novaTuraObjekat.Isporucilac;
            pompomKomanda.Parameters.Add("@SifraSorte", SqlDbType.NVarChar).Value = novaTuraObjekat.Sorta.Sifra;
            pompomKomanda.Parameters.Add("@NetoMasaKg", SqlDbType.Decimal).Value = novaTuraObjekat.NetoMasaKg;
            pompomKomanda.Parameters.Add("@SecerProcenat", SqlDbType.Decimal).Value = novaTuraObjekat.SecerProcenat;
            pompomKomanda.Parameters.Add("@KiselostGl", SqlDbType.Decimal).Value = novaTuraObjekat.KiselostGl;
            pompomKomanda.Parameters.Add("@TemperaturaC", SqlDbType.Decimal).Value = novaTuraObjekat.TemperaturaC;
            pompomKomanda.Parameters.Add("@UdeoOstecenihProcenat", SqlDbType.Decimal).Value = novaTuraObjekat.UdeoOstecenihProcenat;
            pompomKomanda.Parameters.Add("@Usmerenje", SqlDbType.NVarChar).Value = novaTuraObjekat.Usmerenje;

            brojSlogova = pompomKomanda.ExecuteNonQuery();
            pompomKonekcija.Close();
            pompomKonekcija.Dispose();
     
            // 2. varijanta
            return (brojSlogova > 0);

        }

        public bool ObrisiTuru(string oznakaTureZaBrisanje)
        {
            // LOKALNE PROMENLJIVE UVEK NA VRHU
            int brojSlogova = 0;
            // 1. varijanta - skolska 
            //bool uspehSnimanja= false;

            SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
            pomKonekcija.Open();

            SqlCommand pomKomanda = new SqlCommand("ObrisiTuru", pomKonekcija);
            pomKomanda.CommandType = CommandType.StoredProcedure;
            pomKomanda.Parameters.Add("@Oznaka", SqlDbType.NVarChar).Value = oznakaTureZaBrisanje;
            brojSlogova = pomKomanda.ExecuteNonQuery();
            pomKonekcija.Close();
            pomKonekcija.Dispose();

            return (brojSlogova > 0);

        }

        public bool IzmeniTuru(TuraKlasa staraTuraObjekat, TuraKlasa novaTuraObjekat)
        {
            // LOKALNE PROMENLJIVE UVEK NA VRHU
            int brojSlogova = 0;
            // 1. varijanta - skolska 
            //bool uspehSnimanja= false;

            SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
            pomKonekcija.Open();

            SqlCommand pomKomanda = new SqlCommand("IzmeniTuru", pomKonekcija);
            pomKomanda.CommandType = CommandType.StoredProcedure;
            pomKomanda.Parameters.Add("@StaraOznaka", SqlDbType.NVarChar).Value = staraTuraObjekat.Oznaka;
            pomKomanda.Parameters.Add("@Oznaka", SqlDbType.NVarChar).Value = novaTuraObjekat.Oznaka;
            pomKomanda.Parameters.Add("@BrojPrijema", SqlDbType.NVarChar).Value = novaTuraObjekat.BrojPrijema;
            pomKomanda.Parameters.Add("@DatumPrijema", SqlDbType.Date).Value = novaTuraObjekat.DatumPrijema;
            pomKomanda.Parameters.Add("@Isporucilac", SqlDbType.NVarChar).Value = novaTuraObjekat.Isporucilac;
            pomKomanda.Parameters.Add("@SifraSorte", SqlDbType.NVarChar).Value = novaTuraObjekat.Sorta.Sifra;
            pomKomanda.Parameters.Add("@NetoMasaKg", SqlDbType.Decimal).Value = novaTuraObjekat.NetoMasaKg;
            pomKomanda.Parameters.Add("@SecerProcenat", SqlDbType.Decimal).Value = novaTuraObjekat.SecerProcenat;
            pomKomanda.Parameters.Add("@KiselostGl", SqlDbType.Decimal).Value = novaTuraObjekat.KiselostGl;
            pomKomanda.Parameters.Add("@TemperaturaC", SqlDbType.Decimal).Value = novaTuraObjekat.TemperaturaC;
            pomKomanda.Parameters.Add("@UdeoOstecenihProcenat", SqlDbType.Decimal).Value = novaTuraObjekat.UdeoOstecenihProcenat;
            pomKomanda.Parameters.Add("@Usmerenje", SqlDbType.NVarChar).Value = novaTuraObjekat.Usmerenje;
            brojSlogova = pomKomanda.ExecuteNonQuery();
            pomKonekcija.Close();
            pomKonekcija.Dispose();

            return (brojSlogova > 0);

        }

        


    }
}
