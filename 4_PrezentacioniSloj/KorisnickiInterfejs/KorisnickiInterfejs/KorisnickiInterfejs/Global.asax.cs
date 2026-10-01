using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
//
using DBUtils;
using System.Configuration; 

namespace KorisnickiInterfejs
{
    public class Global : System.Web.HttpApplication
    {
        // ATRIBUTI - GLOBALNE PROMENLJIVE ZA CELU APLIKACIJU
        public static KonekcijaKlasa otvorenaKonekcija;
        public static bool uspehKonekcije;

        // NASE PROCEDURE
        public static bool OtvoriKonekcijuDoBazePodataka()
        // ovde se procedura OtvoriKonekciju zove isto kao i metoda klase konekcija
        // to je dozvoljeno
        {
             // OCITAVANJE PARAMETARA KONEKCIJE
            string stringKonekcije = ConfigurationManager.ConnectionStrings["NasaKonekcija"].ConnectionString;


            // KONEKTOVANJE NA BAZU PODATAKA
            otvorenaKonekcija = new KonekcijaKlasa(stringKonekcije);
            uspehKonekcije = otvorenaKonekcija.OtvoriKonekciju();

            // VRACANJE REZULTATA KONEKCIJE
            return uspehKonekcije;
        }


        public void ZatvoriKonekciju()
        {
            otvorenaKonekcija.ZatvoriKonekciju();
        }

        // DOGADJAJI - GLOBALNI ZA CELU APLIKACIJU
        void Application_Start(object sender, EventArgs e)
        {
            RutaKonfiguracija.RegistrujRute(RouteTable.Routes);
            uspehKonekcije = OtvoriKonekcijuDoBazePodataka();

        }

        void Application_End(object sender, EventArgs e)
        {
            ZatvoriKonekciju();

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started

        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }

    }
}
