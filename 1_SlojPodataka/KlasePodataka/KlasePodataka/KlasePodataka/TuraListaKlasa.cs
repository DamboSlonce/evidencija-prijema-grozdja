using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KlasePodataka
{
    public class TuraListaKlasa
    {

        // atributi
        private List<TuraKlasa> _listaTura; 

        // property
        public List<TuraKlasa> ListaTura
        {
            get
            {
                return _listaTura;
            }
            set
            {
                if (this._listaTura != value)
                    this._listaTura = value;
            }
        }

        // konstruktor
        public TuraListaKlasa()
        {
            _listaTura = new List<TuraKlasa>(); 

        }

        // privatne metode

        // javne metode
        public void DodajElementListe(TuraKlasa novaTuraObjekat)
        {
            _listaTura.Add(novaTuraObjekat);
        }

        public void ObrisiElementListe(TuraKlasa turaObjekatZaBrisanje)
        {
            _listaTura.Remove(turaObjekatZaBrisanje);  
        }

        public void ObrisiElementNaPoziciji(int pozicija)
        {
            _listaTura.RemoveAt(pozicija);
        }

        public void IzmeniElementListe(TuraKlasa staraTuraObjekat, TuraKlasa novaTuraObjekat)
        {
            int indexStareTure = 0;
            indexStareTure = _listaTura.IndexOf(staraTuraObjekat);
            _listaTura.RemoveAt(indexStareTure);
            _listaTura.Insert(indexStareTure, novaTuraObjekat);   
        }

           
    }
}
