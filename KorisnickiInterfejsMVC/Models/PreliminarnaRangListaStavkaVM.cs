using System;

namespace KorisnickiInterfejsMVC.Models
{
    public class PreliminarnaRangListaStavkaVM
    {
        public int Pozicija { get; set; }

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

        public decimal UkupnoBodova { get; set; }
        public DateTime DatumObjave { get; set; }
    }
}