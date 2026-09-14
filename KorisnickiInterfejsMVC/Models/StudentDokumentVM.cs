using System;

namespace KorisnickiInterfejsMVC.Models
{
    public class StudentDokumentVM
    {
        public int ID { get; set; }
        public int StudentID { get; set; }
        public int VrstaDokumentaID { get; set; }

        public string NazivDokumenta { get; set; }
        public bool Obavezan { get; set; }

        public bool Podnet { get; set; }
        public DateTime? DatumPodnosenja { get; set; }

        public bool? Ispravan { get; set; }
        public DateTime? DatumProvere { get; set; }

        public string ProverioImeIPrezime { get; set; }
        public string Napomena { get; set; }
    }
}