using System;

namespace KorisnickiInterfejsMVC.Models
{
    public class StudentDokumentaciaVM
    {
        public int ID { get; set; }
        public int StudentID { get; set; }
        public string StudentIme { get; set; }
        public string StudentPrezime { get; set; }
        public string BrojIndeksa { get; set; }
        
        // Document submission status
        public bool SviDokumentiPodneseni { get; set; }
        public DateTime? DatumPodnosenja { get; set; }
        
        // Document verification status
        public bool SviDokumentiProvereni { get; set; }
        public DateTime? DatumProvere { get; set; }
        public string NapomenaProvere { get; set; }
        
        // Data entry status
        public bool SpremnaZaUnos { get; set; }
        
        /// <summary>
        /// Indicates current workflow stage
        /// </summary>
        public string StanjeProcesa
        {
            get
            {
                if (!SviDokumentiPodneseni)
                    return "?ekanje dokumentacije";
                if (!SviDokumentiProvereni)
                    return "Dokumentacija na proveri";
                if (!SpremnaZaUnos)
                    return "Spremna za unos";
                return "Završena";
            }
        }
    }
}
