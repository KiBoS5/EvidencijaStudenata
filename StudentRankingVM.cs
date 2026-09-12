using System;

namespace KorisnickiInterfejsMVC.Models
{
    public class StudentRankingVM
    {
        public int ID { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string BrojIndeksa { get; set; }
        public string StudijskiProgram { get; set; }
        public string GodinaStudija { get; set; }
        public decimal Prosek { get; set; }
        public bool BezRoditelja { get; set; }
        public bool DokumentacijaPrihodaDostavljena { get; set; }
        public decimal UkupnaPrimanjaDoma?instva { get; set; }
        public int Broj?lanovaPorodice { get; set; }
        public int BodoviProsek { get; set; }
        public int BodoviPrimanja { get; set; }
        public int BodoviGodina { get; set; }
        public int UkupnoBodova { get; set; }
        public bool IsEligible { get; set; }
        
        /// <summary>
        /// Calculated property: Average income per family member
        /// </summary>
        public decimal Prose?naPrimanjaPo?lanu
        {
            get
            {
                if (Broj?lanovaPorodice <= 0)
                    return 0;
                return UkupnaPrimanjaDoma?instva / Broj?lanovaPorodice;
            }
        }
    }
}
