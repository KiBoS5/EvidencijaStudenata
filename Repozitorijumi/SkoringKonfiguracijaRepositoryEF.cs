using System;
using System.Data.Entity;
using System.Linq;
using KlasePodataka;
using KlasePodataka.Kontekst;

namespace Repozitorijumi
{
    public class SkoringKonfiguracijaRepositoryEF
        : ISkoringKonfiguracijaRepository
    {
        private readonly string _stringKonekcije;

        public SkoringKonfiguracijaRepositoryEF(
            string stringKonekcije)
        {
            _stringKonekcije = stringKonekcije;
        }

        public SkoringKonfiguracijaKlasa DajKonfiguraciju()
        {
            using (var kontekst =
                new SkoringDbKontekst(_stringKonekcije))
            {
                var konfiguracije = kontekst.SkoringKonfiguracije
                    .AsNoTracking()
                    .Take(2)
                    .ToList();

                if (konfiguracije.Count == 0)
                {
                    throw new InvalidOperationException(
                        "Konfiguracija bodovanja nije uneta.");
                }

                if (konfiguracije.Count > 1)
                {
                    throw new InvalidOperationException(
                        "Postoji više konfiguracija bodovanja. " +
                        "Potrebna je jedna konfiguracija.");
                }

                return konfiguracije[0];
            }
        }

        public void SacuvajKonfiguraciju(
            SkoringKonfiguracijaKlasa konfiguracija)
        {
            if (konfiguracija == null)
            {
                throw new ArgumentNullException(
                    nameof(konfiguracija));
            }

            using (var kontekst =
                new SkoringDbKontekst(_stringKonekcije))
            {
                var postojeca = kontekst.SkoringKonfiguracije
                    .SingleOrDefault(x => x.ID == konfiguracija.ID);

                if (postojeca == null)
                {
                    throw new InvalidOperationException(
                        "Konfiguracija koju pokušavate da izmenite " +
                        "ne postoji.");
                }

                postojeca.MinimalanProsek =
                    konfiguracija.MinimalanProsek;

                postojeca.ProsekZa10Bodova =
                    konfiguracija.ProsekZa10Bodova;

                postojeca.ProsekZa15Bodova =
                    konfiguracija.ProsekZa15Bodova;

                postojeca.ProsekZa20Bodova =
                    konfiguracija.ProsekZa20Bodova;

                postojeca.PrimanjaZa10Bodova =
                    konfiguracija.PrimanjaZa10Bodova;

                postojeca.PrimanjaZa1Bod =
                    konfiguracija.PrimanjaZa1Bod;

                postojeca.BodoviGodinaStudije1 =
                    konfiguracija.BodoviGodinaStudije1;

                postojeca.BodoviGodinaStudije2 =
                    konfiguracija.BodoviGodinaStudije2;

                postojeca.BodoviGodinaStudije3 =
                    konfiguracija.BodoviGodinaStudije3;

                postojeca.BodoviGodinaStudije4 =
                    konfiguracija.BodoviGodinaStudije4;

                postojeca.BodoviMaster =
                    konfiguracija.BodoviMaster;

                postojeca.BezRoditeljaMultiplier =
                    konfiguracija.BezRoditeljaMultiplier;

                postojeca.DatumAzuriranja = DateTime.UtcNow;

                kontekst.SaveChanges();
            }
        }
    }
}