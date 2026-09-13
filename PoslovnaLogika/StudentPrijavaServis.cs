using System;
using System.Threading.Tasks;
using KlasePodataka;
using Repozitorijumi;
using Servisi;

namespace PoslovnaLogika
{
    public class StudentPrijavaServis
    {
        private readonly IStudentRepository _studentRepo;
        private readonly IOgranicenjaKlijent _ogranicenjaKlijent;
        private readonly OgranicenjaKonkursaPravila _pravila;

        public StudentPrijavaServis(
            IStudentRepository studentRepo,
            IOgranicenjaKlijent ogranicenjaKlijent)
        {
            if (studentRepo == null)
            {
                throw new ArgumentNullException(nameof(studentRepo));
            }

            if (ogranicenjaKlijent == null)
            {
                throw new ArgumentNullException(
                    nameof(ogranicenjaKlijent));
            }

            _studentRepo = studentRepo;
            _ogranicenjaKlijent = ogranicenjaKlijent;
            _pravila = new OgranicenjaKonkursaPravila();
        }

        public async Task<int> DodajStudentaAsync(
            StudentKlasa student)
        {
            if (student == null)
            {
                throw new ArgumentNullException(nameof(student));
            }

            // Parametre preuzimamo jednom za ovu prijavu.
            var ogranicenja =
                await _ogranicenjaKlijent
                    .DajOgranicenjaAsync()
                    .ConfigureAwait(false);

            // Provera datuma konkursa u poslovnoj logici.
            _pravila.ProveriDodavanjePrijave(
                ogranicenja,
                DateTimeOffset.UtcNow);

            // DajSve vraća sve prijave, uključujući one
            // koje nemaju ispravnu dokumentaciju.
            int trenutniBrojPrijava =
                _studentRepo.DajSve().Count;

            _pravila.ProveriBrojPrijava(
                ogranicenja,
                trenutniBrojPrijava);

            // Upis se izvršava tek nakon uspešnih provera.
            // SQL procedura ponovo proverava limit
            // unutar transakcije zbog istovremenih zahteva.
            return _studentRepo.DodajStudenta(
                student,
                ogranicenja.MaksimalanBrojPrijava);
        }
    }
}