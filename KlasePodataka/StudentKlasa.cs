using System;

namespace KlasePodataka
{
    public class StudentKlasa
    {
        public int ID { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public string BrojIndeksa { get; set; }
        public string StudijskiProgram { get; set; }
        public string GodinaStudija { get; set; }
        public bool BezRoditelja { get; set; }
        public decimal Prosek { get; set; }
        public bool DokumentacijaPrihodaDostavljena { get; set; }
        public decimal UkupnaPrimanjaDomacinstva { get; set; }
        public int BrojClanovaPorodice { get; set; }
        public int BodoviProsek { get; set; }
        public int BodoviPrimanja { get; set; }
        public int BodoviGodina { get; set; }
        public int UkupnoBodova { get; set; }
        public bool IsEligible { get; set; }

        public bool DokumentacijaSpremnaZaRangiranje { get; set; }
    }
}
