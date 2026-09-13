using System;
using KlasePodataka;
using Repozitorijumi;

namespace PoslovnaLogika
{
    public class SkoringKonfiguracijaServis
    {
        private readonly ISkoringKonfiguracijaRepository _repo;

        public SkoringKonfiguracijaServis(
            ISkoringKonfiguracijaRepository repo)
        {
            if (repo == null)
            {
                throw new ArgumentNullException(nameof(repo));
            }

            _repo = repo;
        }

        public SkoringKonfiguracijaKlasa DajKonfiguraciju()
        {
            var konfiguracija = _repo.DajKonfiguraciju();

            Validiraj(konfiguracija);

            return konfiguracija;
        }

        public void SacuvajKonfiguraciju(
            SkoringKonfiguracijaKlasa konfiguracija)
        {
            Validiraj(konfiguracija);

            _repo.SacuvajKonfiguraciju(konfiguracija);
        }

        private static void Validiraj(
            SkoringKonfiguracijaKlasa konfiguracija)
        {
            if (konfiguracija == null)
            {
                throw new ArgumentException(
                    "Konfiguracija bodovanja nije dostupna.");
            }

            if (konfiguracija.ID <= 0)
            {
                throw new ArgumentException(
                    "Neispravan ID konfiguracije.");
            }

            ProveriDecimal(
                konfiguracija.MinimalanProsek,
                5m,
                10m,
                "Minimalan prosek");

            ProveriDecimal(
                konfiguracija.ProsekZa10Bodova,
                5m,
                10m,
                "Prosek za 10 bodova");

            ProveriDecimal(
                konfiguracija.ProsekZa15Bodova,
                5m,
                10m,
                "Prosek za 15 bodova");

            ProveriDecimal(
                konfiguracija.ProsekZa20Bodova,
                5m,
                10m,
                "Prosek za 20 bodova");

            if (konfiguracija.MinimalanProsek >
                konfiguracija.ProsekZa10Bodova)
            {
                throw new ArgumentException(
                    "Minimalan prosek ne sme biti veći " +
                    "od praga za 10 bodova.");
            }

            if (konfiguracija.ProsekZa10Bodova >=
                    konfiguracija.ProsekZa15Bodova ||
                konfiguracija.ProsekZa15Bodova >=
                    konfiguracija.ProsekZa20Bodova)
            {
                throw new ArgumentException(
                    "Pragovi za 10, 15 i 20 bodova " +
                    "moraju biti strogo rastući.");
            }

            ProveriDecimal(
                konfiguracija.PrimanjaZa10Bodova,
                0m,
                9999999999999.99m,
                "Primanja za 10 bodova");

            ProveriDecimal(
                konfiguracija.PrimanjaZa1Bod,
                0m,
                9999999999999.99m,
                "Primanja za 1 bod");

            if (konfiguracija.PrimanjaZa1Bod <=
                konfiguracija.PrimanjaZa10Bodova)
            {
                throw new ArgumentException(
                    "Prag primanja za 1 bod mora biti veći " +
                    "od praga za 10 bodova.");
            }

            ProveriBodove(
                konfiguracija.BodoviGodinaStudije1,
                "Prva godina");

            ProveriBodove(
                konfiguracija.BodoviGodinaStudije2,
                "Druga godina");

            ProveriBodove(
                konfiguracija.BodoviGodinaStudije3,
                "Treća godina");

            ProveriBodove(
                konfiguracija.BodoviGodinaStudije4,
                "Četvrta godina");

            ProveriBodove(
                konfiguracija.BodoviMaster,
                "Master / doktorske studije");

            ProveriDecimal(
                konfiguracija.BezRoditeljaMultiplier,
                0.10m,
                1.00m,
                "Multiplikator za studente bez roditelja");
        }

        private static void ProveriDecimal(
            decimal vrednost,
            decimal minimum,
            decimal maksimum,
            string naziv)
        {
            if (vrednost < minimum || vrednost > maksimum)
            {
                throw new ArgumentException(
                    naziv + " mora biti između " +
                    minimum + " i " + maksimum + ".");
            }

            if (decimal.Round(vrednost, 2) != vrednost)
            {
                throw new ArgumentException(
                    naziv + " može imati najviše dve decimale.");
            }
        }

        private static void ProveriBodove(
            int bodovi,
            string naziv)
        {
            if (bodovi < 0 || bodovi > 20)
            {
                throw new ArgumentException(
                    naziv + ": broj bodova mora biti između 0 i 20.");
            }
        }
    }
}