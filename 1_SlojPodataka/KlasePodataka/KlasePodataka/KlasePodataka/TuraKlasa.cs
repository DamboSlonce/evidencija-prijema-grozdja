using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KlasePodataka
{
    public class TuraKlasa
    {
        // atributi
        private string _oznaka;
        private string _brojPrijema;
        private DateTime _datumPrijema;
        private string _isporucilac;
        private SortaKlasa _sortaObjekat;
        private decimal _netoMasaKg;
        private decimal _secerProcenat;
        private decimal _kiselostGl;
        private decimal _temperaturaC;
        private decimal _udeoOstecenihProcenat;
        private string _usmerenje;

        // property
        public string Oznaka
        {
            get { return _oznaka; }
            set { _oznaka = value; }
        }

        

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

        public SortaKlasa Sorta
        {
            get { return _sortaObjekat; }
            set { _sortaObjekat = value; }
        }

        public decimal NetoMasaKg
        {
            get { return _netoMasaKg; }
            set { _netoMasaKg = value; }
        }

        public decimal SecerProcenat
        {
            get { return _secerProcenat; }
            set { _secerProcenat = value; }
        }

        public decimal KiselostGl
        {
            get { return _kiselostGl; }
            set { _kiselostGl = value; }
        }

        public decimal TemperaturaC
        {
            get { return _temperaturaC; }
            set { _temperaturaC = value; }
        }

        public decimal UdeoOstecenihProcenat
        {
            get { return _udeoOstecenihProcenat; }
            set { _udeoOstecenihProcenat = value; }
        }

        public string Usmerenje
        {
            get { return _usmerenje; }
            set { _usmerenje = value; }
        }
    }
}
