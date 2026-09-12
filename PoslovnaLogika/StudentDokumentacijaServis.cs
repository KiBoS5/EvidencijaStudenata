using System;
using System.Collections.Generic;
using KlasePodataka;
using Repozitorijumi;

namespace PoslovnaLogika
{
    public class StudentDokumentacijaServis
    {
        private readonly
            IStudentDokumentacijaRepository _repo;

        public StudentDokumentacijaServis(
            IStudentDokumentacijaRepository repo)
        {
            if (repo == null)
            {
                throw new ArgumentNullException("repo");
            }

            _repo = repo;
        }

        public List<StudentDokumentacijaKlasa>
            DajDokumentacijuStudenata()
        {
            return _repo.DajDokumentacijuStudenata();
        }

        public void OznaciDokumentacijuPodnetom(
            int studentID)
        {
            ValidirajStudentID(studentID);

            _repo.OznaciDokumentacijuPodnetom(
                studentID);
        }

        public void ProveriDokumentacijuStudenta(
            int studentID,
            bool dokumentacijaIspravna,
            string napomena,
            int proverioKorisnikID)
        {
            ValidirajStudentID(studentID);

            if (proverioKorisnikID <= 0)
            {
                throw new ArgumentException(
                    "Nije moguće utvrditi zaposlenog koji vrši proveru.");
            }

            string sredjenaNapomena =
                string.IsNullOrWhiteSpace(napomena)
                    ? null
                    : napomena.Trim();

            if (sredjenaNapomena != null &&
                sredjenaNapomena.Length > 500)
            {
                throw new ArgumentException(
                    "Napomena može imati najviše 500 karaktera.");
            }

            if (!dokumentacijaIspravna &&
                string.IsNullOrWhiteSpace(sredjenaNapomena))
            {
                throw new ArgumentException(
                    "Napomena je obavezna kada dokumentacija nije ispravna.");
            }

            _repo.ProveriDokumentacijuStudenta(
                studentID,
                dokumentacijaIspravna,
                sredjenaNapomena,
                proverioKorisnikID);
        }

        private void ValidirajStudentID(
            int studentID)
        {
            if (studentID <= 0)
            {
                throw new ArgumentException(
                    "Neispravan ID studenta.");
            }
        }
    }
}