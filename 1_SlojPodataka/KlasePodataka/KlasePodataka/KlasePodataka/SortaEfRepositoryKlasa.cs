using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace KlasePodataka
{
    public class SortaEfRepositoryKlasa
    {
        private string _stringKonekcije;

        public SortaEfRepositoryKlasa(string noviStringKonekcije)
        {
            _stringKonekcije = noviStringKonekcije;
        }

        public List<SortaEfKlasa> DajSve()
        {
            using (VinarijaKontekstKlasa kontekst = new VinarijaKontekstKlasa(this._stringKonekcije))
            {
                return kontekst.Sorte.ToList();
            }
        }

        public SortaEfKlasa DajPoId(string sifra)
        {
            using (VinarijaKontekstKlasa kontekst = new VinarijaKontekstKlasa(this._stringKonekcije))
            {
                return kontekst.Sorte.Find(sifra);
            }
        }

        public DataSet DajSveKaoDataSet()
        {
            DataSet ds = new DataSet();
            DataTable tabela = new DataTable("SORTA");
            tabela.Columns.Add("Sifra");
            tabela.Columns.Add("Naziv");
            List<SortaEfKlasa> lista = this.DajSve();
            for (int i = 0; i < lista.Count; i++)
            {
                tabela.Rows.Add(lista[i].Sifra, lista[i].Naziv);
            }
            ds.Tables.Add(tabela);
            return ds;
        }

        public bool SnimiNovi(SortaKlasa sorta)
        {
            using (VinarijaKontekstKlasa kontekst = new VinarijaKontekstKlasa(this._stringKonekcije))
            {
                SortaEfKlasa nova = new SortaEfKlasa();
                nova.Sifra = sorta.Sifra;
                nova.Naziv = sorta.Naziv;
                kontekst.Sorte.Add(nova);
                return kontekst.SaveChanges() > 0;
            }
        }
    }
}
