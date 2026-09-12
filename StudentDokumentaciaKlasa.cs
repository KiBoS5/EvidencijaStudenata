using System;

namespace KlasePodataka
{
    /// <summary>
    /// Tracks document submission status for scholarship applications
    /// Enables multi-step workflow: Document Submission ? Verification ? Data Entry
    /// </summary>
    public class StudentDokumentaciaKlasa
    {
        public int ID { get; set; }
        public int StudentID { get; set; }
        
        // Document submission statuses
        public bool DokumentacijaProverena { get; set; } = false;
        public DateTime? DatumPodnosenja { get; set; }
        public DateTime? DatumProvere { get; set; }
        
        // Document verification notes
        public string NapomenaProvere { get; set; }
        
        // Indicates if all documents have been submitted
        public bool SviDokumentiPodneseni { get; set; } = false;
        
        // Indicates if all documents have been verified
        public bool SviDokumentiProvereni { get; set; } = false;
        
        // Indicates if student data can be entered (all docs verified)
        public bool SpremnaZaUnos { get; set; } = false;
        
        // Audit fields
        public DateTime DatumKreiranja { get; set; } = DateTime.Now;
        public DateTime DatumAžuriranja { get; set; } = DateTime.Now;
        
        /// <summary>
        /// Check if student is ready for data entry (all documents verified)
        /// </summary>
        public bool MozeSeUnositi()
        {
            return SviDokumentiProvereni && SpremnaZaUnos;
        }
    }
}
