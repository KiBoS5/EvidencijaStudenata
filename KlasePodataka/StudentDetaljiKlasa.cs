using System.Collections.Generic;

namespace KlasePodataka
{
    public class StudentDetaljiKlasa
    {
        public StudentKlasa Student { get; set; }

        public List<StudentDokumentKlasa> Dokumenti { get; set; }

        public StudentDetaljiKlasa()
        {
            Dokumenti = new List<StudentDokumentKlasa>();
        }
    }
}