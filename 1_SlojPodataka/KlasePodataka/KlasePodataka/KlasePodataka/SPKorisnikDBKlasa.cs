using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using System.Data;
using System.Data.SqlClient;

namespace KlasePodataka
{
    public class SPKorisnikDBKlasa
    {

        #region Atributi
            private string _stringKonekcije;
        #endregion

        #region Property  
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
        #endregion

            #region Konstruktor
        
                // 2. nacin prijema vrednosti stringa konekcije spolja i dodele atributu
                public SPKorisnikDBKlasa(string noviStringKonekcije)
                // OVO JE DOBRO JER OBAVEZUJE DA SE PRILIKOM INSTANCIRANJA OVE KLASE
                // MORA OBEZBEDITI STRING KONEKCIJE
                {
                    _stringKonekcije = noviStringKonekcije; 
                }
            #endregion



        #region JavneMetode
                public DataSet DajKorisnikaPoKorisnickomImenuISifri(string novoKorisnickoIme, string novaSifra)
                {
                    // MOGU biti jos neke procedure, mogu SE VRATITI VREDNOSTI I U LISTU, DATA TABLE...
                    DataSet PodaciDataSet = new DataSet();

                    SqlConnection pomKonekcija = new SqlConnection(_stringKonekcije);
                    pomKonekcija.Open();
                    SqlCommand pomKomanda = new SqlCommand("DajKorisnikaPoKorisnickomImenuISifri", pomKonekcija);
                    pomKomanda.Parameters.Add("@KorisnickoIme", SqlDbType.NVarChar).Value = novoKorisnickoIme;
                    pomKomanda.Parameters.Add("@Sifra", SqlDbType.NVarChar).Value = novaSifra;
                    pomKomanda.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter pomDataAdapter = new SqlDataAdapter();
                    pomDataAdapter.SelectCommand = pomKomanda;
                    pomDataAdapter.Fill(PodaciDataSet);
                    pomKonekcija.Close();
                    pomKonekcija.Dispose();

                    return PodaciDataSet;
                } // kraj metode
        #endregion
    } // kraj klase
} // kraj namespace
