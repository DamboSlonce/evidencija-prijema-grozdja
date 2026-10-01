using System;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web;

namespace ParametriKvaliteta
{
    public class OgranicenjaRest : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string akcija = context.Request["akcija"] ?? "";
            string sifra = context.Request["sifra"] ?? "";
            DataSet dsOgranicenja = new DataSet();
            dsOgranicenja.ReadXml(context.Server.MapPath("~/XML/OgranicenjaKvaliteta.XML"));

            string json = "{\"greska\":\"nepoznata akcija\"}";
            if (akcija == "DajMinSecer")
            {
                decimal minSecer = this.DajVrednost(dsOgranicenja, sifra, 1);
                json = "{\"minSecer\":" + minSecer.ToString(CultureInfo.InvariantCulture) + "}";
            }
            else if (akcija == "DajMaxUdeoOstecenih")
            {
                decimal maxUdeo = this.DajVrednost(dsOgranicenja, sifra, 2);
                json = "{\"maxUdeoOstecenih\":" + maxUdeo.ToString(CultureInfo.InvariantCulture) + "}";
            }
            else if (akcija == "DajSvaOgranicenja")
            {
                json = this.TabelaUJson(dsOgranicenja.Tables[0]);
            }

            context.Response.ContentType = "application/json; charset=utf-8";
            byte[] buffer = Encoding.UTF8.GetBytes(json);
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
        }

        private decimal DajVrednost(DataSet dsOgranicenja, string sifra, int indeksKolone)
        {
            DataRow[] result = dsOgranicenja.Tables[0].Select("SifraSorte='" + sifra.Replace("'", "''") + "'");
            return decimal.Parse(result[0].ItemArray[indeksKolone].ToString(), CultureInfo.InvariantCulture);
        }

        private string TabelaUJson(DataTable tabela)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                if (i > 0)
                {
                    sb.Append(",");
                }
                sb.Append("{");
                for (int j = 0; j < tabela.Columns.Count; j++)
                {
                    if (j > 0)
                    {
                        sb.Append(",");
                    }
                    string vrednost = tabela.Rows[i][j].ToString().Replace("\\", "\\\\").Replace("\"", "\\\"");
                    sb.Append("\"" + tabela.Columns[j].ColumnName + "\":\"" + vrednost + "\"");
                }
                sb.Append("}");
            }
            sb.Append("]");
            return sb.ToString();
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}
