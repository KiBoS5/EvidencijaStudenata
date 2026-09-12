using System;

namespace KorisnickiInterfejsMVC.Models
{
    public class StudentDokumentacijaVM
    {
        public int DokumentacijaID { get; set; }
        public int StudentID { get; set; }

        public string StudentIme { get; set; }
        public string StudentPrezime { get; set; }
        public string BrojIndeksa { get; set; }

        public bool SviDokumentiPodneseni { get; set; }
        public DateTime? DatumPodnosenja { get; set; }

        public bool SviDokumentiProvereni { get; set; }
        public DateTime? DatumProvere { get; set; }

        public bool? DokumentacijaIspravna { get; set; }
        public string NapomenaProvere { get; set; }

        public bool SpremnaZaUnos { get; set; }

        public int? ProverioKorisnikID { get; set; }
        public string ProverioImeIPrezime { get; set; }
        public string ProverioKorisnickoIme { get; set; }

        public string StanjeProcesa { get; set; }
    }
}