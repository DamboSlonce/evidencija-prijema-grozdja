using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
//
using System.Data; 

namespace ParametriKvaliteta
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class OgranicenjaKvaliteta : System.Web.Services.WebService
    {

        [WebMethod]
        public DataSet DajSvaOgranicenja()
        {
            DataSet dsOgranicenja = new DataSet();
            dsOgranicenja.ReadXml(Server.MapPath("~/") + "XML/OgranicenjaKvaliteta.XML");

            return dsOgranicenja;
        }


        [WebMethod]
        public decimal DajMinSecer(string pomSifraSorte)
        {
            decimal minSecer = 0;
            DataSet dsOgranicenja = new DataSet();
            dsOgranicenja.ReadXml(Server.MapPath("~/") + "XML/OgranicenjaKvaliteta.XML");
            // filtriranje dataset-a
            DataRow[] result = dsOgranicenja.Tables[0].Select("SifraSorte='" + pomSifraSorte + "'");
            minSecer = decimal.Parse (result[0].ItemArray[1].ToString());

            return minSecer;
        }

        [WebMethod]
        public decimal DajMaxUdeoOstecenih(string pomSifraSorte)
        {
            decimal maxUdeoOstecenih = 0;
            DataSet dsOgranicenja = new DataSet();
            dsOgranicenja.ReadXml(Server.MapPath("~/") + "XML/OgranicenjaKvaliteta.XML");
            // filtriranje dataset-a
            DataRow[] result = dsOgranicenja.Tables[0].Select("SifraSorte='" + pomSifraSorte + "'");
            maxUdeoOstecenih = decimal.Parse(result[0].ItemArray[2].ToString());

            return maxUdeoOstecenih;
        }

       
    }
}
