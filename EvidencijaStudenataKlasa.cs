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
        private readonly string _konekcija;
        private SkoringKonfiguraciaKlasa _konfiguracija;

        public EvidencijaStudenataKlasa(IStudentRepository repo, string konekcija = "")
        {
            _repo = repo;
            _konekcija = konekcija;
            
            // Load configuration if connection string provided
            if (!string.IsNullOrWhiteSpace(konekcija))
            {
                var dbKonfig = new SPSkoringKonfiguraciaDBKlasa(konekcija);
                _konfiguracija = dbKonfig.DajKonfiguraciju();
            }
            else
            {
                _konfiguracija = new SkoringKonfiguraciaKlasa();
            }
        }

        public StudentKlasa PripremiZaEvidenciju(StudentKlasa student)
        {
            student.BodoviProsek = IzracunajBodoveProseka(student.Prosek);
            student.BodoviPrimanja = IzracunajBodovePrimanja(student.UkupnaPrimanjaDoma?instva, student.Broj?lanovaPorodice, student.BezRoditelja);
            student.BodoviGodina = IzracunajBodoveGodine(student.GodinaStudija);
            student.UkupnoBodova = student.BodoviProsek + student.BodoviPrimanja + student.BodoviGodina;
            return student;
        }

        public bool MozeSeKonkuretati(StudentKlasa student)
        {
            return student.Prosek >= _konfiguracija.MinimalanProsek && student.DokumentacijaPrihodaDostavljena;
        }

        // ...existing code...
        public int IzracunajBodoveProseka(decimal prosek)
        {
            if (prosek >= _konfiguracija.ProsekZa20Bodova) return 20;
            if (prosek >= _konfiguracija.ProsekZa15Bodova) return 15;
            if (prosek >= _konfiguracija.ProsekZa10Bodova) return 10;
            return 0;
        }

        public int IzracunajBodoveGodine(string godinaStudija)
        {
            if (string.IsNullOrWhiteSpace(godinaStudija))
                return 0;

            string normalized = godinaStudija.Trim().ToLowerInvariant();

            if (normalized.StartsWith("1")) return _konfiguracija.BodoviGodinaStudije1;
            if (normalized.StartsWith("2")) return _konfiguracija.BodoviGodinaStudije2;
            if (normalized.StartsWith("3")) return _konfiguracija.BodoviGodinaStudije3;
            if (normalized.StartsWith("4")) return _konfiguracija.BodoviGodinaStudije4;
            if (normalized.Contains("master") || normalized.Contains("doktor") || normalized.Contains("doktorske") || normalized.Contains("doktors"))
                return _konfiguracija.BodoviMaster;

            return 0;
        }

        /// <summary>
        /// Calculate financial status points using configurable income thresholds
        /// Uses linear scale: income range maps to 1-10 points (lower income = higher points)
        /// </summary>
        public int IzracunajBodovePrimanja(decimal ukupnaPrimanja, int broj?lanova, bool bezRoditelja)
        {
            if (broj?lanova <= 0)
                return 0;

            decimal prosecnaPrimanja = ukupnaPrimanja / broj?lanova;

            // Apply orphan status multiplier if applicable
            if (bezRoditelja)
            {
                prosecnaPrimanja *= _konfiguracija.BezRoditeljaMultiplier;
            }

            // Calculate score on a linear scale
            if (prosecnaPrimanja <= _konfiguracija.PrimanjaZa10Bodova)
                return 10;
            
            if (prosecnaPrimanja >= _konfiguracija.PrimanjaZa1Bod)
                return 1;
            
            // Linear interpolation between 10 and 1 points
            decimal range = _konfiguracija.PrimanjaZa1Bod - _konfiguracija.PrimanjaZa10Bodova;
            decimal ratio = (prosecnaPrimanja - _konfiguracija.PrimanjaZa10Bodova) / range;
            decimal score = 10 - (ratio * 9);
            
            return Convert.ToInt32(Math.Round(score));
        }

        public List<StudentKlasa> Rangiraj(StudentFilter filter = null)
        {
            var lista = _repo.GetAll();

            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.GodinaStudija))
                    lista = lista.Where(x => x.GodinaStudija == filter.GodinaStudija).ToList();
                if (filter.IsEligible.HasValue)
                    lista = lista.Where(x => x.IsEligible == filter.IsEligible.Value).ToList();
            }

            foreach (var student in lista)
            {
                PripremiZaEvidenciju(student);
            }

            return lista
                .OrderByDescending(x => x.UkupnoBodova)
                .ThenByDescending(x => x.Prosek)
                .ToList();
        }
    }

    public class StudentFilter
    {
        public string GodinaStudija { get; set; }
        public bool? IsEligible { get; set; }
    }
}
