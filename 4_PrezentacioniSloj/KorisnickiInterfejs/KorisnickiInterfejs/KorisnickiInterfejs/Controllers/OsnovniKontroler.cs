using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.Mvc;
using PrezentacionaLogika;

namespace KorisnickiInterfejs.Controllers
{
    public class OsnovniKontroler : Controller
    {
        protected string StringKonekcije
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["NasaKonekcija"].ConnectionString;
            }
        }

        protected string KorisnikImePrezime
        {
            get
            {
                if (Session["KorisnikImePrezime"] == null)
                {
                    return "";
                }
                return Session["KorisnikImePrezime"].ToString();
            }
        }

        protected string NormalizujFilter(string filter)
        {
            if (string.IsNullOrEmpty(filter) || filter == "0")
            {
                return "";
            }
            return filter;
        }

        protected List<SelectListItem> DajStavkeSorti()
        {
            FormaTuraUnosKlasa forma = new FormaTuraUnosKlasa(this.StringKonekcije);
            DataSet sorte = forma.DajPodatkeZaCombo();
            List<SelectListItem> lista = new List<SelectListItem>();
            lista.Add(new SelectListItem { Text = "Izaberite...", Value = "Izaberite..." });
            if (sorte != null && sorte.Tables.Count > 0)
            {
                for (int i = 0; i < sorte.Tables[0].Rows.Count; i++)
                {
                    string naziv = sorte.Tables[0].Rows[i].ItemArray[1].ToString();
                    lista.Add(new SelectListItem { Text = naziv, Value = naziv });
                }
            }
            return lista;
        }
    }
}
