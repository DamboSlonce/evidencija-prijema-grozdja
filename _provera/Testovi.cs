using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;
using KlasePodataka;
using PrezentacionaLogika;
using PoslovnaLogika;

namespace ProveraVinarije
{
    class Program
    {
        static int fail = 0;
        static int pass = 0;
        const string Konekcija = @"Data Source=(localdb)\MSSQLLocalDB; Initial Catalog=VP2026Vinarija; Integrated Security=True;";
        static string xmlPath;

        static void Ok(string name, bool uslov, string detalj)
        {
            if (uslov)
            {
                pass++;
                Console.WriteLine("PASS  " + name + "  " + detalj);
            }
            else
            {
                fail++;
                Console.WriteLine("FAIL  " + name + "  " + detalj);
            }
        }

        static void Main(string[] args)
        {
            xmlPath = args.Length > 0 ? args[0] : @"XML\OgranicenjaKvaliteta.XML";
            HttpListener listener = null;
            try
            {
                listener = PokreniSoap();
                Console.WriteLine("SOAP stub na http://localhost:1718/");
            }
            catch (Exception greska)
            {
                Console.WriteLine("SOAP stub NIJE pokrenut: " + greska.Message);
            }
            try
            {
                TestBaza();
                TestLogin();
                TestSorta();
                TestSpisakTura();
                if (listener != null)
                {
                    TestPoslovnoPraviloIUnos();
                    TestJedinstvenost();
                    TestSoapDirektno();
                    TestRestDirektno();
                    TestMasterDetailINasledjivanje();
                }
                else
                {
                    fail += 12;
                    Console.WriteLine("FAIL  SOAP/REST zavisni testovi preskoceni");
                }
            }
            catch (Exception greska)
            {
                fail++;
                Console.WriteLine("FAIL  izuzetak: " + greska);
            }
            finally
            {
                if (listener != null)
                {
                    try { listener.Stop(); listener.Close(); } catch { }
                }
            }

            Console.WriteLine();
            Console.WriteLine("Rezultat: {0} prolaz, {1} pad", pass, fail);
            Environment.Exit(fail == 0 ? 0 : 1);
        }

        static HttpListener PokreniSoap()
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:1718/");
            listener.Start();
            Thread nit = new Thread(delegate()
            {
                while (listener.IsListening)
                {
                    HttpListenerContext ctx = null;
                    try { ctx = listener.GetContext(); }
                    catch { break; }
                    try { ObradiSoap(ctx); }
                    catch (Exception greska)
                    {
                        try
                        {
                            byte[] buf = Encoding.UTF8.GetBytes(greska.ToString());
                            ctx.Response.StatusCode = 500;
                            ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                            ctx.Response.Close();
                        }
                        catch { }
                    }
                }
            });
            nit.IsBackground = true;
            nit.Start();
            return listener;
        }

        static void ObradiSoap(HttpListenerContext ctx)
        {
            if (string.Equals(ctx.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                ObradiRest(ctx);
                return;
            }

            string telo;
            using (StreamReader r = new StreamReader(ctx.Request.InputStream, Encoding.UTF8))
                telo = r.ReadToEnd();

            string akcija = ctx.Request.Headers["SOAPAction"] ?? "";
            akcija = akcija.Replace("\"", "");
            DataSet ds = new DataSet();
            ds.ReadXml(xmlPath);

            string odgovor;
            if (akcija.IndexOf("DajMinSecer") >= 0)
            {
                string sifra = IzvuciParametar(telo, "pomSifraSorte");
                DataRow[] rows = ds.Tables[0].Select("SifraSorte='" + sifra + "'");
                string vrednost = rows[0][1].ToString().Replace(',', '.');
                odgovor = SoapOdgovor("DajMinSecer", vrednost);
            }
            else if (akcija.IndexOf("DajMaxUdeoOstecenih") >= 0)
            {
                string sifra = IzvuciParametar(telo, "pomSifraSorte");
                DataRow[] rows = ds.Tables[0].Select("SifraSorte='" + sifra + "'");
                string vrednost = rows[0][2].ToString().Replace(',', '.');
                odgovor = SoapOdgovor("DajMaxUdeoOstecenih", vrednost);
            }
            else
            {
                odgovor = SoapOdgovor("DajSvaOgranicenja", "0");
            }

            byte[] buffer = Encoding.UTF8.GetBytes(odgovor);
            ctx.Response.ContentType = "text/xml; charset=utf-8";
            ctx.Response.ContentLength64 = buffer.Length;
            ctx.Response.OutputStream.Write(buffer, 0, buffer.Length);
            ctx.Response.Close();
        }

        static void ObradiRest(HttpListenerContext ctx)
        {
            DataSet ds = new DataSet();
            ds.ReadXml(xmlPath);
            string akcija = ctx.Request.QueryString["akcija"] ?? "";
            string sifra = ctx.Request.QueryString["sifra"] ?? "";
            string json = "{\"greska\":\"nepoznata akcija\"}";
            if (akcija == "DajMinSecer")
            {
                DataRow[] rows = ds.Tables[0].Select("SifraSorte='" + sifra + "'");
                string vrednost = rows[0][1].ToString().Replace(',', '.');
                json = "{\"minSecer\":" + vrednost + "}";
            }
            else if (akcija == "DajMaxUdeoOstecenih")
            {
                DataRow[] rows = ds.Tables[0].Select("SifraSorte='" + sifra + "'");
                string vrednost = rows[0][2].ToString().Replace(',', '.');
                json = "{\"maxUdeoOstecenih\":" + vrednost + "}";
            }
            byte[] buffer = Encoding.UTF8.GetBytes(json);
            ctx.Response.ContentType = "application/json; charset=utf-8";
            ctx.Response.ContentLength64 = buffer.Length;
            ctx.Response.OutputStream.Write(buffer, 0, buffer.Length);
            ctx.Response.Close();
        }

        static string IzvuciParametar(string xml, string ime)
        {
            int a = xml.IndexOf(ime + ">");
            if (a < 0) return "";
            int start = xml.IndexOf(">", a) + 1;
            int end = xml.IndexOf("<", start);
            return xml.Substring(start, end - start);
        }

        static string SoapOdgovor(string metoda, string vrednost)
        {
            return "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">" +
                "<soap:Body>" +
                "<" + metoda + "Response xmlns=\"http://tempuri.org/\">" +
                "<" + metoda + "Result>" + vrednost + "</" + metoda + "Result>" +
                "</" + metoda + "Response>" +
                "</soap:Body></soap:Envelope>";
        }

        static void TestBaza()
        {
            using (SqlConnection c = new SqlConnection(Konekcija))
            {
                c.Open();
                SqlCommand cmd = new SqlCommand("select count(*) from Sorta; select count(*) from Tura; select count(*) from Korisnik;", c);
                SqlDataAdapter a = new SqlDataAdapter();
                a.SelectCommand = new SqlCommand("select (select count(*) from Sorta) as s, (select count(*) from Tura) as t, (select count(*) from Korisnik) as k, (select count(*) from PRIJEM) as p", c);
                DataSet ds = new DataSet();
                a.Fill(ds);
                int s = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                int t = Convert.ToInt32(ds.Tables[0].Rows[0][1]);
                int k = Convert.ToInt32(ds.Tables[0].Rows[0][2]);
                int p = Convert.ToInt32(ds.Tables[0].Rows[0][3]);
                Ok("Baza.Sorta", s >= 3, "sorti=" + s);
                Ok("Baza.Tura", t >= 3, "tura=" + t);
                Ok("Baza.Korisnik", k >= 1, "korisnika=" + k);
                Ok("Baza.Prijem", p >= 1, "prijema=" + p);
            }
        }

        static void TestLogin()
        {
            FormaLoginKlasa login = new FormaLoginKlasa(Konekcija);
            login.KorisnickoIme = "admin";
            login.Sifra = "admin";
            Ok("Login.admin", login.VazeciKorisnik(), login.DajImePrezimeKorisnika());

            login.KorisnickoIme = "admin";
            login.Sifra = "pogresna";
            Ok("Login.pogresnaSifra", !login.VazeciKorisnik(), "odbijen");
        }

        static void TestSorta()
        {
            FormaSortaUnosKlasa unos = new FormaSortaUnosKlasa(Konekcija);
            unos.Sifra = "X";
            unos.Naziv = "Xinomavro";
            bool snimljeno = unos.SnimiPodatke();
            Ok("Sorta.unos", snimljeno, "Xinomavro");

            SPSortaDBKlasa db = new SPSortaDBKlasa(Konekcija);
            DataSet ds = db.DajSortePoNazivu("Xinomavro");
            Ok("Sorta.citanje", ds.Tables[0].Rows.Count == 1, "redova=" + ds.Tables[0].Rows.Count);

            bool obrisano = db.ObrisiSortu("X");
            Ok("Sorta.brisanje", obrisano, "X obrisana");
        }

        static void TestSpisakTura()
        {
            FormaTuraSpisakKlasa spisak = new FormaTuraSpisakKlasa(Konekcija);
            DataSet svi = spisak.DajPodatkeZaGrid("");
            Ok("Tura.spisakSvi", svi.Tables[0].Rows.Count >= 3, "redova=" + svi.Tables[0].Rows.Count);

            DataSet filter = spisak.DajPodatkeZaGrid("Petar Jovanović");
            Ok("Tura.filterIsporucilac", filter.Tables[0].Rows.Count >= 3, "redova=" + filter.Tables[0].Rows.Count);

            DataSet prazan = spisak.DajPodatkeZaGrid("Nepostojeci");
            Ok("Tura.filterPrazan", prazan.Tables[0].Rows.Count == 0, "redova=" + prazan.Tables[0].Rows.Count);
        }

        static void TestPoslovnoPraviloIUnos()
        {
            UsmerenjePreradeKlasa pravilo = new UsmerenjePreradeKlasa(Konekcija);

            string u1 = pravilo.OdrediUsmerenje("Rizling", 21.4m, 3m);
            Ok("Pravilo.kvalitetnoVino", u1 == "kvalitetno vino", u1);

            string u2 = pravilo.OdrediUsmerenje("Rizling", 17.1m, 3m);
            Ok("Pravilo.nizakSecer", u2 == "destilat", u2);

            string u3 = pravilo.OdrediUsmerenje("Rizling", 21.4m, 12m);
            Ok("Pravilo.osteceneBobice", u3 == "destilat", u3);

            FormaTuraUnosKlasa unos = new FormaTuraUnosKlasa(Konekcija);
            unos.Oznaka = "T-TEST1";
            unos.BrojPrijema = "PG-TEST-1";
            unos.DatumPrijema = "2026-09-13";
            unos.Isporucilac = "Test Isporucilac";
            unos.NazivSorte = "Rizling";
            unos.NetoMasaKg = "100";
            unos.SecerProcenat = "17,1";
            unos.KiselostGl = "8,2";
            unos.TemperaturaC = "26";
            unos.UdeoOstecenihProcenat = "12";
            unos.Usmerenje = "kvalitetno vino";
            string poruka = unos.SnimiPodatke();
            bool automatizam = poruka.IndexOf("destilat") >= 0 && poruka.IndexOf("Uspesno") >= 0;
            Ok("Tura.unosDestilat", automatizam, poruka);

            SPTuraDBKlasa turaDb = new SPTuraDBKlasa(Konekcija);
            DataSet sacuvano = turaDb.DajTuruPoOznaci("T-TEST1");
            string usmerenje = sacuvano.Tables[0].Rows[0]["Usmerenje"].ToString();
            Ok("Tura.sacuvanoUsmerenje", usmerenje == "destilat", usmerenje);

            FormaTuraUnosKlasa unos2 = new FormaTuraUnosKlasa(Konekcija);
            unos2.Oznaka = "T-TEST2";
            unos2.BrojPrijema = "PG-TEST-2";
            unos2.DatumPrijema = "2026-09-13";
            unos2.Isporucilac = "Test Isporucilac";
            unos2.NazivSorte = "Rizling";
            unos2.NetoMasaKg = "200";
            unos2.SecerProcenat = "21,4";
            unos2.KiselostGl = "6,8";
            unos2.TemperaturaC = "22";
            unos2.UdeoOstecenihProcenat = "3";
            unos2.Usmerenje = "";
            string poruka2 = unos2.SnimiPodatke();
            DataSet sacuvano2 = turaDb.DajTuruPoOznaci("T-TEST2");
            string usmerenje2 = sacuvano2.Tables[0].Rows[0]["Usmerenje"].ToString();
            Ok("Tura.unosKvalitetno", usmerenje2 == "kvalitetno vino" && poruka2.IndexOf("Uspesno") >= 0, usmerenje2 + " | " + poruka2);

            turaDb.ObrisiTuru("T-TEST1");
            turaDb.ObrisiTuru("T-TEST2");
            using (SqlConnection c = new SqlConnection(Konekcija))
            {
                c.Open();
                SqlCommand cmd = new SqlCommand("delete from PRIJEM where BrojPrijema in (N'PG-TEST-1', N'PG-TEST-2')", c);
                cmd.ExecuteNonQuery();
            }
        }

        static void TestJedinstvenost()
        {
            FormaTuraUnosKlasa unos = new FormaTuraUnosKlasa(Konekcija);
            unos.Oznaka = "T-01";
            unos.BrojPrijema = "PG-DUP";
            unos.DatumPrijema = "2026-09-13";
            unos.Isporucilac = "Neko";
            unos.NazivSorte = "Rizling";
            unos.NetoMasaKg = "1";
            unos.SecerProcenat = "21";
            unos.KiselostGl = "7";
            unos.TemperaturaC = "20";
            unos.UdeoOstecenihProcenat = "1";
            string poruka = unos.SnimiPodatke();
            Ok("Tura.duplikatOznake", poruka.IndexOf("NIJE JEDINSTVEN") >= 0, poruka);

            FormaTuraUnosKlasa prazan = new FormaTuraUnosKlasa(Konekcija);
            prazan.Oznaka = "";
            prazan.NazivSorte = "Izaberite...";
            string porukaPrazno = prazan.SnimiPodatke();
            Ok("Tura.prazanUnos", porukaPrazno.IndexOf("NIJE SVE POPUNJENO") >= 0, porukaPrazno);
        }

        static void TestSoapDirektno()
        {
            ParametriKvalitetaKlasa p = new ParametriKvalitetaKlasa();
            decimal minR = p.DajMinSecerZaSortu("R");
            decimal maxR = p.DajMaxUdeoOstecenihZaSortu("R");
            Ok("SOAP.minSecerR", minR == 19m, minR.ToString(CultureInfo.InvariantCulture));
            Ok("SOAP.maxOstecenihR", maxR == 10m, maxR.ToString(CultureInfo.InvariantCulture));
        }

        static void TestRestDirektno()
        {
            ParametriKvalitetaRestKlasa rest = new ParametriKvalitetaRestKlasa();
            decimal minR = rest.DajMinSecerZaSortu("R");
            decimal maxR = rest.DajMaxUdeoOstecenihZaSortu("R");
            Ok("REST.minSecerR", minR == 19m, minR.ToString(CultureInfo.InvariantCulture));
            Ok("REST.maxOstecenihR", maxR == 10m, maxR.ToString(CultureInfo.InvariantCulture));
        }

        static void TestMasterDetailINasledjivanje()
        {
            TuraRepositoryKlasa turaRepo = new TuraRepositoryKlasa(Konekcija);
            DataSet nasledjeno = turaRepo.DajSveTurePrekoNasledjeneKlase();
            Ok("Nasledjivanje.TabelaKlasa", nasledjeno.Tables[0].Rows.Count >= 3, "redova=" + nasledjeno.Tables[0].Rows.Count);

            SortaRepositoryKlasa sortaRepo = new SortaRepositoryKlasa(Konekcija);
            DataSet efSorte = sortaRepo.DajSveSortePrekoEntityFramework();
            Ok("EntityFramework.Sorta", efSorte.Tables[0].Rows.Count >= 3, "redova=" + efSorte.Tables[0].Rows.Count);

            PrijemKlasa prijem = new PrijemKlasa();
            prijem.BrojPrijema = "PG-MD-1";
            prijem.DatumPrijema = DateTime.Parse("2026-09-13");
            prijem.Isporucilac = "MD Isporucilac";

            TuraKlasa tura = new TuraKlasa();
            tura.Oznaka = "T-MD1";
            tura.BrojPrijema = "PG-MD-1";
            tura.DatumPrijema = prijem.DatumPrijema;
            tura.Isporucilac = prijem.Isporucilac;
            tura.NetoMasaKg = 100;
            tura.SecerProcenat = 21;
            tura.KiselostGl = 7;
            tura.TemperaturaC = 20;
            tura.UdeoOstecenihProcenat = 2;
            tura.Usmerenje = "kvalitetno vino";
            SortaKlasa sorta = new SortaKlasa();
            sorta.Sifra = "R";
            sorta.Naziv = "Rizling";
            tura.Sorta = sorta;

            System.Collections.Generic.List<TuraKlasa> lista = new System.Collections.Generic.List<TuraKlasa>();
            lista.Add(tura);

            PrijemRepositoryKlasa prijemRepo = new PrijemRepositoryKlasa(Konekcija);
            bool snimljeno = prijemRepo.SnimiPrijemSaTurama(prijem, lista);
            DataSet turePrijema = prijemRepo.DajTurePoBrojuPrijema("PG-MD-1");
            Ok("Prijem.masterDetailTransakcija", snimljeno && turePrijema.Tables[0].Rows.Count >= 1, "ture=" + turePrijema.Tables[0].Rows.Count);

            SPTuraDBKlasa turaDb = new SPTuraDBKlasa(Konekcija);
            turaDb.ObrisiTuru("T-MD1");
            using (SqlConnection c = new SqlConnection(Konekcija))
            {
                c.Open();
                SqlCommand cmd = new SqlCommand("delete from PRIJEM where BrojPrijema=N'PG-MD-1'", c);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
