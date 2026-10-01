using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KlasePodataka
{
    public class SortaListaKlasa
    {
        // atributi
        private List<SortaKlasa> _listaSorti; 

        // property
        public List<SortaKlasa> ListaSorti
        {
            get
            {
                return _listaSorti;
            }
            set
            {
                if (this._listaSorti != value)
                    this._listaSorti = value;
            }
        }

        // konstruktor
        public SortaListaKlasa()
        {
            _listaSorti = new List<SortaKlasa>(); 

        }

        // privatne metode

        // javne metode
        public void DodajElementListe(SortaKlasa novaSortaObjekat)
        {
            _listaSorti.Add(novaSortaObjekat);
        }

        public void ObrisiElementListe(SortaKlasa sortaObjekatZaBrisanje)
        {
            _listaSorti.Remove(sortaObjekatZaBrisanje);  
        }

        public void ObrisiElementNaPoziciji(int pozicija)
        {
            _listaSorti.RemoveAt(pozicija);
        }

        public void IzmeniElementListe(SortaKlasa staraSortaObjekat, SortaKlasa novaSortaObjekat)
        {
            int indexStareSorte = 0;
            indexStareSorte = _listaSorti.IndexOf(staraSortaObjekat);  
            _listaSorti.RemoveAt(indexStareSorte); 
            _listaSorti.Insert(indexStareSorte, novaSortaObjekat);   
        }

           

     




    }
}
