using System;

namespace KlasePodataka
{
    public class PreliminarnaRangListaKlasa
    {
        public int ID { get; set; }
        public int Pozicija { get; set; }
        public int StudentID { get; set; }

        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string BrojIndeksa { get; set; }

        public string StudijskiProgram { get; set; }
        public string GodinaStudija { get; set; }

        public decimal Prosek { get; set; }
        public bool BezRoditelja { get; set; }

        public bool DokumentacijaPrihodaDostavljena
        {
            get;
            set;
        }

        public decimal UkupnaPrimanjaDomacinstva
        {
            get;
            set;
        }

        public int UkupnoBodova { get; set; }

        public DateTime DatumObjave { get; set; }
        public int ObjavioKorisnikID { get; set; }
    }
}