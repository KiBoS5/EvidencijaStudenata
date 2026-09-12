using System.Collections.Generic;
using KlasePodataka;

namespace Repozitorijumi
{
    public class StudentDokumentacijaRepositorySP
        : IStudentDokumentacijaRepository
    {
        private readonly string _stringKonekcije;

        public StudentDokumentacijaRepositorySP(
            string stringKonekcije)
        {
            _stringKonekcije = stringKonekcije;
        }

        public List<StudentDokumentacijaKlasa>
            DajDokumentacijuStudenata()
        {
            SPStudentDokumentacijaDBKlasa db =
                new SPStudentDokumentacijaDBKlasa(
                    _stringKonekcije);

            return db.DajDokumentacijuStudenata();
        }

        public void OznaciDokumentacijuPodnetom(
            int studentID)
        {
            SPStudentDokumentacijaDBKlasa db =
                new SPStudentDokumentacijaDBKlasa(
                    _stringKonekcije);

            db.OznaciDokumentacijuPodnetom(
                studentID);
        }

        public void ProveriDokumentacijuStudenta(
            int studentID,
            bool dokumentacijaIspravna,
            string napomena,
            int proverioKorisnikID)
        {
            SPStudentDokumentacijaDBKlasa db =
                new SPStudentDokumentacijaDBKlasa(
                    _stringKonekcije);

            db.ProveriDokumentacijuStudenta(
                studentID,
                dokumentacijaIspravna,
                napomena,
                proverioKorisnikID);
        }
    }
}