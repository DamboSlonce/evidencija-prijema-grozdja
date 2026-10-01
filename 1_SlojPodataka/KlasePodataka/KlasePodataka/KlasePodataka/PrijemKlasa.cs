using System;

namespace KlasePodataka
{
    public class PrijemKlasa
    {
        private string _brojPrijema;
        private DateTime _datumPrijema;
        private string _isporucilac;

        public string BrojPrijema
        {
            get { return _brojPrijema; }
            set { _brojPrijema = value; }
        }

        public DateTime DatumPrijema
        {
            get { return _datumPrijema; }
            set { _datumPrijema = value; }
        }

        public string Isporucilac
        {
            get { return _isporucilac; }
            set { _isporucilac = value; }
        }
    }
}
