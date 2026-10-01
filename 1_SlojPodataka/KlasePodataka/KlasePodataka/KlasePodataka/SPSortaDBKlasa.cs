using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using System.Data;
using System.Data.SqlClient;

namespace KlasePodataka
{
    public class SPSortaDBKlasa
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
            set // OVO NIJE DOBRO, MOZE SE STRING KONEKCIJE STAVITI NA PRAZAN STRING
            {
                if (this._stringKonekcije != value)
                    this._stringKonekcije = value;
            }
        }
        // konstruktor
        // 2. nacin prijema vrednosti stringa konekcije spolja i dodele atributu
        public SPSortaDBKlasa(string noviStringKonekcije)
        // OVO JE DOBRO JER OBAVEZUJE DA SE PRILIKOM INSTANCIRANJA OVE KLASE
        // MORA OBEZBEDITI STRING KONEKCIJE
        {
            _stringKonekcije = noviStringKonekcije; 
        }

        // privatne metode

        // javne metode
        public DataSet DajSveSorte()
        {
            // MOGU biti jos neke procedure, mogu SE VRATITI VREDNOSTI I U LISTU, DATA TABLE...
            DataSet PodaciDataSet = new DataSet();

            SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
            pomKonekcija.Open();
            SqlCommand pomKomanda = new SqlCommand("DajSveSorte", pomKonekcija);
            pomKomanda.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = pomKomanda;
            adapter.Fill(PodaciDataSet);
            pomKonekcija.Close();
            pomKonekcija.Dispose();
            
            return PodaciDataSet;
        }

        public string DajNazivPremaIDSorte(string IDSorteFilter)
        {
            string pomNazivSorte = "";

            // MOGU biti jos neke procedure, mogu SE VRATITI VREDNOSTI I U LISTU, DATA TABLE...
            DataSet PodaciDataSet = new DataSet();

            SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
            pomKonekcija.Open();
            SqlCommand pomKomanda = new SqlCommand("DajSortuPoSifri", pomKonekcija);
            pomKomanda.CommandType = CommandType.StoredProcedure;
            pomKomanda.Parameters.Add("@SifraSorte", SqlDbType.Char).Value = IDSorteFilter;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = pomKomanda;
            adapter.Fill(PodaciDataSet);
            pomKonekcija.Close();
            pomKonekcija.Dispose();

            pomNazivSorte = PodaciDataSet.Tables[0].Rows[0].ItemArray[1].ToString();

            return pomNazivSorte;
        }

        public string DajSifruSortePoNazivu(string nazivSorteFilter)
        {
            // MOGU biti jos neke procedure, mogu SE VRATITI VREDNOSTI I U LISTU, DATA TABLE...
            DataSet PodaciDataSet = new DataSet();

            SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
            pomKonekcija.Open();
            SqlCommand pomKomanda = new SqlCommand("DajSortuPoNazivu", pomKonekcija);
            pomKomanda.CommandType = CommandType.StoredProcedure;
            pomKomanda.Parameters.Add("@NazivSorte", SqlDbType.NVarChar).Value = nazivSorteFilter;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = pomKomanda;
            adapter.Fill(PodaciDataSet);
            pomKonekcija.Close();
            pomKonekcija.Dispose();

            return PodaciDataSet.Tables[0].Rows[0].ItemArray[0].ToString() ;
        }



        public DataSet DajSortePoNazivu(string nazivSorteFilter)
        {
            // MOGU biti jos neke procedure, mogu SE VRATITI VREDNOSTI I U LISTU, DATA TABLE...
            DataSet dsPodaci = new DataSet();

            SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
            pomKonekcija.Open();
            SqlCommand pomKomanda = new SqlCommand("DajSortuPoNazivu", pomKonekcija);
            pomKomanda.CommandType = CommandType.StoredProcedure;
            pomKomanda.Parameters.Add("@NazivSorte", SqlDbType.NVarChar).Value = nazivSorteFilter;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = pomKomanda;
            adapter.Fill(dsPodaci);
            pomKonekcija.Close();
            pomKonekcija.Dispose();

            return dsPodaci;
        }

        // overloading metoda - isto se zove, ima drugaciji parametar
        public DataSet DajSortePoNazivu(SortaKlasa sortaObjekatZaFilter)
        {
            // MOGU biti jos neke procedure, mogu SE VRATITI VREDNOSTI I U LISTU, DATA TABLE...
            DataSet dsPodaci = new DataSet();

            SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
            pomKonekcija.Open();
            SqlCommand pomKomanda = new SqlCommand("DajSortuPoNazivu", pomKonekcija);
            pomKomanda.CommandType = CommandType.StoredProcedure;
            pomKomanda.Parameters.Add("@NazivSorte", SqlDbType.NVarChar).Value = sortaObjekatZaFilter.Naziv;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = pomKomanda;
            adapter.Fill(dsPodaci);
            pomKonekcija.Close();
            pomKonekcija.Dispose();

            return dsPodaci;
        }

        public bool SnimiNovuSortu(SortaKlasa novaSortaObjekat)
        {
            // LOKALNE PROMENLJIVE UVEK NA VRHU
            int brojSlogova =0;
            // 1. varijanta - skolska 
            //bool uspehSnimanja= false;

            SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
            pomKonekcija.Open();

            SqlCommand pomKomanda = new SqlCommand("DodajNovuSortu", pomKonekcija);
            pomKomanda.CommandType = CommandType.StoredProcedure;
            pomKomanda.Parameters.Add("@Sifra", SqlDbType.Char).Value = novaSortaObjekat.Sifra;
            pomKomanda.Parameters.Add("@Naziv", SqlDbType.NVarChar).Value = novaSortaObjekat.Naziv;
           
            brojSlogova = pomKomanda.ExecuteNonQuery();
            pomKonekcija.Close();
            pomKonekcija.Dispose();

                      
            
            return (brojSlogova > 0);

            //3. varijanta
            // NEMA SMISLA, ISTO KAO 2. VARIJANTA
            //return (brojSlogova > 0) ? true : false;
            
            //4. varijanta - NEGACIJA NEGACIJE, NIJE RAZUMLJIVO 
            //return (brojSlogova == 0) ? false : true;
            
        }

        public bool ObrisiSortu(SortaKlasa sortaObjekatZaBrisanje)
        {
            // LOKALNE PROMENLJIVE UVEK NA VRHU
            int brojSlogova = 0;
            // 1. varijanta - skolska 
            //bool uspehSnimanja= false;

            SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
            pomKonekcija.Open();

            SqlCommand pomKomanda = new SqlCommand("ObrisiSortu", pomKonekcija);
            pomKomanda.CommandType = CommandType.StoredProcedure;
            pomKomanda.Parameters.Add("@Sifra", SqlDbType.Char).Value = sortaObjekatZaBrisanje.Sifra;
            brojSlogova = pomKomanda.ExecuteNonQuery();
            pomKonekcija.Close();
            pomKonekcija.Dispose();

            return (brojSlogova > 0);

         }

        // method overloading - ista procedura sa razlicitim parametrom
        public bool ObrisiSortu(string sifraSorteZaBrisanje)
        {
            // LOKALNE PROMENLJIVE UVEK NA VRHU
            int brojSlogova = 0;
            // 1. varijanta - skolska 
            //bool uspehSnimanja= false;

            SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
            pomKonekcija.Open();

            SqlCommand pomKomanda = new SqlCommand("ObrisiSortu", pomKonekcija);
            pomKomanda.CommandType = CommandType.StoredProcedure;
            pomKomanda.Parameters.Add("@Sifra", SqlDbType.Char).Value = sifraSorteZaBrisanje;
            brojSlogova = pomKomanda.ExecuteNonQuery();
            pomKonekcija.Close();
            pomKonekcija.Dispose();

            return (brojSlogova > 0);

        }

        public bool IzmeniSortu(SortaKlasa staraSortaObjekat, SortaKlasa novaSortaObjekat)
        {
            // LOKALNE PROMENLJIVE UVEK NA VRHU
            int brojSlogova = 0;
            // 1. varijanta - skolska 
            //bool uspehSnimanja= false;

            SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
            pomKonekcija.Open();

            SqlCommand pomKomanda = new SqlCommand("IzmeniSortu", pomKonekcija);
            pomKomanda.CommandType = CommandType.StoredProcedure;
            pomKomanda.Parameters.Add("@StaraSifra", SqlDbType.Char).Value = staraSortaObjekat.Sifra;
            pomKomanda.Parameters.Add("@Sifra", SqlDbType.Char).Value = novaSortaObjekat.Sifra;
            pomKomanda.Parameters.Add("@Naziv", SqlDbType.NVarChar).Value = novaSortaObjekat.Naziv;
            brojSlogova = pomKomanda.ExecuteNonQuery();
            pomKonekcija.Close();
            pomKonekcija.Dispose();

            return (brojSlogova > 0);

        }

        // method overloading - ista metoda, samo drugaciji parametri
        public bool IzmeniSortu(string sifraStareSorte, SortaKlasa novaSortaObjekat)
        {
            // LOKALNE PROMENLJIVE UVEK NA VRHU
            int brojSlogova = 0;
            // 1. varijanta - skolska 
            //bool uspehSnimanja= false;

            SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
            pomKonekcija.Open();

            SqlCommand pomKomanda = new SqlCommand("IzmeniSortu", pomKonekcija);
            pomKomanda.CommandType = CommandType.StoredProcedure;
            pomKomanda.Parameters.Add("@StaraSifra", SqlDbType.Char).Value = sifraStareSorte;
            pomKomanda.Parameters.Add("@Sifra", SqlDbType.Char).Value = novaSortaObjekat.Sifra;
            pomKomanda.Parameters.Add("@Naziv", SqlDbType.NVarChar).Value = novaSortaObjekat.Naziv;
            brojSlogova = pomKomanda.ExecuteNonQuery();
            pomKonekcija.Close();
            pomKonekcija.Dispose();

            return (brojSlogova > 0);

        }


    }
}
