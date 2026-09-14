using System;
using KlasePodataka;
using Repozitorijumi;

namespace PrezentacionaLogika
{
    public class FormaStudentDetaljiKlasa
    {
        private readonly IStudentRepository _studentRepo;
        private readonly IStudentDokumentRepository _dokumentRepo;

        public FormaStudentDetaljiKlasa(
            IStudentRepository studentRepo,
            IStudentDokumentRepository dokumentRepo)
        {
            if (studentRepo == null)
            {
                throw new ArgumentNullException(
                    nameof(studentRepo));
            }

            if (dokumentRepo == null)
            {
                throw new ArgumentNullException(
                    nameof(dokumentRepo));
            }

            _studentRepo = studentRepo;
            _dokumentRepo = dokumentRepo;
        }

        public StudentDetaljiKlasa DajDetalje(int studentID)
        {
            if (studentID <= 0)
            {
                throw new ArgumentException(
                    "Neispravan ID studenta.");
            }

            StudentKlasa student =
                _studentRepo.DajStudentaPoID(studentID);

            if (student == null)
            {
                return null;
            }

            return new StudentDetaljiKlasa
            {
                Student = student,
                Dokumenti = _dokumentRepo
                    .DajDokumenteStudenta(studentID)
            };
        }
    }
}