using System.Collections.Generic;
using KlasePodataka;

namespace Repozitorijumi
{
    public class StudentDokumentRepositorySP
        : IStudentDokumentRepository
    {
        private readonly string _stringKonekcije;

        public StudentDokumentRepositorySP(string stringKonekcije)
        {
            _stringKonekcije = stringKonekcije;
        }

        public List<StudentDokumentKlasa> DajDokumenteStudenta(
            int studentID)
        {
            var db = new SPStudentDokumentDBKlasa(
                _stringKonekcije);

            return db.DajDokumenteStudenta(studentID);
        }

        public void OznaciPodnetim(
            int studentID,
            int dokumentID)
        {
            var db = new SPStudentDokumentDBKlasa(
                _stringKonekcije);

            db.OznaciPodnetim(studentID, dokumentID);
        }

        public void ProveriDokument(
            int studentID,
            int dokumentID,
            bool ispravan,
            int proverioKorisnikID,
            string napomena)
        {
            var db = new SPStudentDokumentDBKlasa(
                _stringKonekcije);

            db.ProveriDokument(
                studentID,
                dokumentID,
                ispravan,
                proverioKorisnikID,
                napomena);
        }
    }
}