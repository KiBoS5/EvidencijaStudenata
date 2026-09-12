using System;

namespace KlasePodataka
{
    public class KorisnikKlasa
    {
        public int ID { get; set; }

        public string Ime { get; set; }
        public string Prezime { get; set; }

        public string KorisnickoIme { get; set; }

        public string LozinkaHash { get; set; }

        public TipKorisnika TipKorisnika { get; set; }

        public bool Aktivan { get; set; }

        public DateTime DatumKreiranja { get; set; }
        public DateTime? DatumPoslednjePrijave { get; set; }

        public string ImeIPrezime
        {
            get
            {
                return Ime + " " + Prezime;
            }
        }
    }
}