using System;
using System.Net.Mail;
using KlasePodataka;
using Repozitorijumi;

namespace PoslovnaLogika
{
    public class StudentIzmenaServis
    {
        private readonly IStudentRepository _repo;

        public StudentIzmenaServis(IStudentRepository repo)
        {
            if (repo == null)
            {
                throw new ArgumentNullException(nameof(repo));
            }

            _repo = repo;
        }

        public void IzmeniStudenta(StudentKlasa student)
        {
            if (student == null)
            {
                throw new ArgumentNullException(nameof(student));
            }

            ProveriID(student.ID);

            student.Ime =
                ProveriTekst(student.Ime, "Ime", 100);

            student.Prezime =
                ProveriTekst(student.Prezime, "Prezime", 100);

            student.Email =
                ProveriTekst(student.Email, "Email", 255);

            student.Telefon =
                ProveriTekst(student.Telefon, "Telefon", 30);

            student.BrojIndeksa =
                ProveriTekst(student.BrojIndeksa, "Broj indeksa", 50);

            student.StudijskiProgram =
                ProveriTekst(
                    student.StudijskiProgram,
                    "Studijski program",
                    150);

            student.GodinaStudija =
                ProveriTekst(
                    student.GodinaStudija,
                    "Godina studija",
                    50);

            if (student.DatumRodjenja == default(DateTime) ||
                student.DatumRodjenja.Date > DateTime.UtcNow.Date)
            {
                throw new ArgumentException(
                    "Unesite ispravan datum rođenja.");
            }

            if (!EmailJeIspravan(student.Email))
            {
                throw new ArgumentException(
                    "Unesite ispravnu email adresu.");
            }

            if (student.Prosek < 6m ||
                student.Prosek > 10m ||
                decimal.Round(student.Prosek, 2) != student.Prosek)
            {
                throw new ArgumentException(
                    "Prosek mora biti između 6 i 10, sa najviše dve decimale.");
            }

            if (student.UkupnaPrimanjaDomacinstva < 0m ||
                student.UkupnaPrimanjaDomacinstva > 10000000m ||
                decimal.Round(student.UkupnaPrimanjaDomacinstva, 2) !=
                    student.UkupnaPrimanjaDomacinstva)
            {
                throw new ArgumentException(
                    "Primanja moraju biti između 0 i 10.000.000, " +
                    "sa najviše dve decimale.");
            }

            if (student.BrojClanovaPorodice < 1 ||
                student.BrojClanovaPorodice > 20)
            {
                throw new ArgumentException(
                    "Broj članova porodice mora biti između 1 i 20.");
            }

            _repo.IzmeniStudenta(student);
        }

        public void ObrisiStudenta(int studentID)
        {
            ProveriID(studentID);

            _repo.ObrisiStudenta(studentID);
        }

        private static void ProveriID(int studentID)
        {
            if (studentID <= 0)
            {
                throw new ArgumentException(
                    "Neispravan ID studenta.");
            }
        }

        private static string ProveriTekst(
            string vrednost,
            string naziv,
            int maksimalnaDuzina)
        {
            if (string.IsNullOrWhiteSpace(vrednost))
            {
                throw new ArgumentException(
                    naziv + " je obavezno polje.");
            }

            string tekst = vrednost.Trim();

            if (tekst.Length > maksimalnaDuzina)
            {
                throw new ArgumentException(
                    naziv + " može imati najviše " +
                    maksimalnaDuzina + " karaktera.");
            }

            return tekst;
        }

        private static bool EmailJeIspravan(string email)
        {
            try
            {
                var adresa = new MailAddress(email);

                return string.Equals(
                    adresa.Address,
                    email,
                    StringComparison.OrdinalIgnoreCase);
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}