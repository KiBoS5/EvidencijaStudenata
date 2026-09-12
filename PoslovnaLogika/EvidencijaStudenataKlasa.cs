using KlasePodataka;
using Repozitorijumi;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PoslovnaLogika
{
    public class EvidencijaStudenataKlasa
    {
        private readonly IStudentRepository _repo;
        private readonly SkoringKonfiguracijaServis _skoringServis;

        public EvidencijaStudenataKlasa(
            IStudentRepository repo,
            SkoringKonfiguracijaServis skoringServis)
        {
            if (repo == null)
                throw new ArgumentNullException(nameof(repo));

            if (skoringServis == null)
                throw new ArgumentNullException(nameof(skoringServis));

            _repo = repo;
            _skoringServis = skoringServis;
        }

        public StudentKlasa PripremiZaEvidenciju(StudentKlasa student)
        {
            var konfiguracija = _skoringServis.DajKonfiguraciju();

            return PripremiZaEvidenciju(student, konfiguracija);
        }

        private StudentKlasa PripremiZaEvidenciju(
            StudentKlasa student,
            SkoringKonfiguracijaKlasa konfiguracija)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));

            student.BodoviProsek = IzracunajBodoveProseka(
                student.Prosek,
                konfiguracija);

            student.BodoviPrimanja = IzracunajBodovePrimanja(
                student.UkupnaPrimanjaDomacinstva,
                student.BrojClanovaPorodice,
                student.BezRoditelja,
                konfiguracija);

            student.BodoviGodina = IzracunajBodoveGodine(
                student.GodinaStudija,
                konfiguracija);

            student.UkupnoBodova =
                student.BodoviProsek +
                student.BodoviPrimanja +
                student.BodoviGodina;

            student.IsEligible = MozeSeKonkuretati(
                student,
                konfiguracija);

            return student;
        }

        public bool MozeSeKonkuretati(StudentKlasa student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));

            var konfiguracija = _skoringServis.DajKonfiguraciju();

            return MozeSeKonkuretati(student, konfiguracija);
        }

        private static bool MozeSeKonkuretati(
            StudentKlasa student,
            SkoringKonfiguracijaKlasa konfiguracija)
        {
            return student.Prosek >= konfiguracija.MinimalanProsek
                && student.DokumentacijaSpremnaZaRangiranje;
        }

        public int IzracunajBodoveProseka(decimal prosek)
        {
            var konfiguracija = _skoringServis.DajKonfiguraciju();

            return IzracunajBodoveProseka(prosek, konfiguracija);
        }

        private static int IzracunajBodoveProseka(
            decimal prosek,
            SkoringKonfiguracijaKlasa konfiguracija)
        {
            if (prosek >= konfiguracija.ProsekZa20Bodova)
                return 20;

            if (prosek >= konfiguracija.ProsekZa15Bodova)
                return 15;

            if (prosek >= konfiguracija.ProsekZa10Bodova)
                return 10;

            return 0;
        }

        public int IzracunajBodoveGodine(string godinaStudija)
        {
            var konfiguracija = _skoringServis.DajKonfiguraciju();

            return IzracunajBodoveGodine(
                godinaStudija,
                konfiguracija);
        }

        private static int IzracunajBodoveGodine(
            string godinaStudija,
            SkoringKonfiguracijaKlasa konfiguracija)
        {
            if (string.IsNullOrWhiteSpace(godinaStudija))
                return 0;

            string godina = godinaStudija.Trim().ToLowerInvariant();

            // Proveravamo nivo studija pre broja godine,
            // da "1. godina master" dobije bodove za master.
            if (godina.Contains("master") ||
                godina.Contains("doktor") ||
                godina.Contains("phd") ||
                godina.Contains("ph.d"))
            {
                return konfiguracija.BodoviMaster;
            }

            if (godina.StartsWith("1"))
                return konfiguracija.BodoviGodinaStudije1;

            if (godina.StartsWith("2"))
                return konfiguracija.BodoviGodinaStudije2;

            if (godina.StartsWith("3"))
                return konfiguracija.BodoviGodinaStudije3;

            if (godina.StartsWith("4"))
                return konfiguracija.BodoviGodinaStudije4;

            return 0;
        }

        public int IzracunajBodovePrimanja(
            decimal ukupnaPrimanja,
            int brojČlanova,
            bool bezRoditelja)
        {
            var konfiguracija = _skoringServis.DajKonfiguraciju();

            return IzracunajBodovePrimanja(
                ukupnaPrimanja,
                brojČlanova,
                bezRoditelja,
                konfiguracija);
        }

        private static int IzracunajBodovePrimanja(
            decimal ukupnaPrimanja,
            int brojClanova,
            bool bezRoditelja,
            SkoringKonfiguracijaKlasa konfiguracija)
        {
            if (brojClanova <= 0 || ukupnaPrimanja < 0)
                return 0;

            decimal primanjaPoClanu = ukupnaPrimanja / brojClanova;

            if (bezRoditelja)
            {
                primanjaPoClanu *=
                    konfiguracija.BezRoditeljaMultiplier;
            }

            if (primanjaPoClanu <= konfiguracija.PrimanjaZa10Bodova)
                return 10;

            if (primanjaPoClanu >= konfiguracija.PrimanjaZa1Bod)
                return 1;

            decimal raspon =
                konfiguracija.PrimanjaZa1Bod -
                konfiguracija.PrimanjaZa10Bodova;

            decimal udeo =
                (primanjaPoClanu - konfiguracija.PrimanjaZa10Bodova)
                / raspon;

            decimal bodovi = 10m - (udeo * 9m);

            return (int)Math.Floor(bodovi);
        }

        public List<StudentKlasa> Rangiraj(StudentFilter filter = null)
        {
            // Jedno učitavanje konfiguracije za celu rang-listu.
            var konfiguracija = _skoringServis.DajKonfiguraciju();

            var lista = _repo.GetAll()
                .Where(x => x.DokumentacijaSpremnaZaRangiranje)
                .ToList();

            if (filter != null &&
                !string.IsNullOrWhiteSpace(filter.GodinaStudija))
            {
                string godinaStudija = filter.GodinaStudija.Trim();

                lista = lista
                    .Where(x => x.GodinaStudija == godinaStudija)
                    .ToList();
            }

            foreach (var student in lista)
            {
                PripremiZaEvidenciju(student, konfiguracija);
            }

            if (filter != null && filter.IsEligible.HasValue)
            {
                lista = lista
                    .Where(x => x.IsEligible == filter.IsEligible.Value)
                    .ToList();
            }

            return lista
                .OrderByDescending(x => x.UkupnoBodova)
                .ThenByDescending(x => x.Prosek)
                .ThenBy(x => x.ID)
                .ToList();
        }
    }

    public class StudentFilter
    {
        public string GodinaStudija { get; set; }
        public bool? IsEligible { get; set; }
    }
}