using System;
using System.Collections.Generic;
using KlasePodataka;
using Repozitorijumi;

namespace PoslovnaLogika
{
    public class StudentDokumentServis
    {
        private readonly IStudentDokumentRepository _repo;

        public StudentDokumentServis(
            IStudentDokumentRepository repo)
        {
            if (repo == null)
            {
                throw new ArgumentNullException(nameof(repo));
            }

            _repo = repo;
        }

        public List<StudentDokumentKlasa> DajDokumenteStudenta(
            int studentID)
        {
            if (studentID <= 0)
            {
                throw new ArgumentException(
                    "Neispravan ID studenta.");
            }

            return _repo.DajDokumenteStudenta(studentID);
        }

        public void OznaciPodnetim(
    int studentID,
    int dokumentID)
        {
            ProveriIdentifikatore(studentID, dokumentID);

            _repo.OznaciPodnetim(studentID, dokumentID);
        }

        public void ProveriDokument(
            int studentID,
            int dokumentID,
            bool? ispravan,
            int proverioKorisnikID,
            string napomena)
        {
            ProveriIdentifikatore(studentID, dokumentID);

            if (!ispravan.HasValue)
            {
                throw new ArgumentException(
                    "Izaberite rezultat provere.");
            }

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

            if (!ispravan.Value && sredjenaNapomena == null)
            {
                throw new ArgumentException(
                    "Napomena je obavezna kada dokument nije ispravan.");
            }

            _repo.ProveriDokument(
                studentID,
                dokumentID,
                ispravan.Value,
                proverioKorisnikID,
                sredjenaNapomena);
        }

        private static void ProveriIdentifikatore(
            int studentID,
            int dokumentID)
        {
            if (studentID <= 0)
            {
                throw new ArgumentException(
                    "Neispravan ID studenta.");
            }

            if (dokumentID <= 0)
            {
                throw new ArgumentException(
                    "Neispravan ID dokumenta.");
            }
        }
    }
}