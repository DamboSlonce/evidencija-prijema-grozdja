using System.Data;
using KlasePodataka;

namespace PrezentacionaLogika
{
    public class FormaPrijemPrikazKlasa
    {
        private string _stringKonekcije;

        public FormaPrijemPrikazKlasa(string noviStringKonekcije)
        {
            _stringKonekcije = noviStringKonekcije;
        }

        public DataSet DajSvePrijeme()
        {
            PrijemRepositoryKlasa prijemRepo = new PrijemRepositoryKlasa(this._stringKonekcije);
            return prijemRepo.DajSvePrijeme();
        }

        public DataSet DajTurePoBrojuPrijema(string brojPrijema)
        {
            PrijemRepositoryKlasa prijemRepo = new PrijemRepositoryKlasa(this._stringKonekcije);
            return prijemRepo.DajTurePoBrojuPrijema(brojPrijema);
        }
    }
}
