using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using PrezentacionaLogika;

namespace KorisnickiInterfejs
{
    public class PodaciRest : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string akcija = context.Request["akcija"] ?? "";
            string filter = context.Request["filter"] ?? "";
            string konekcija = ConfigurationManager.ConnectionStrings["NasaKonekcija"].ConnectionString;
            string json = "{\"greska\":\"nepoznata akcija\"}";

            if (akcija == "DajSveTure")
            {
                FormaTuraSpisakKlasa spisak = new FormaTuraSpisakKlasa(konekcija);
                DataSet ds = spisak.DajPodatkeZaGrid(filter);
                json = this.TabelaUJson(ds.Tables[0]);
            }
            else if (akcija == "DajSvePrijeme")
            {
                FormaPrijemPrikazKlasa prikaz = new FormaPrijemPrikazKlasa(konekcija);
                DataSet ds = prikaz.DajSvePrijeme();
                json = this.TabelaUJson(ds.Tables[0]);
            }
            else if (akcija == "DajTurePoBrojuPrijema")
            {
                FormaPrijemPrikazKlasa prikaz = new FormaPrijemPrikazKlasa(konekcija);
                DataSet ds = prikaz.DajTurePoBrojuPrijema(filter);
                json = this.TabelaUJson(ds.Tables[0]);
            }

            context.Response.ContentType = "application/json; charset=utf-8";
            byte[] buffer = Encoding.UTF8.GetBytes(json);
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
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
