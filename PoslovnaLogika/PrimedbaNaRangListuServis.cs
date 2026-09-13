using System;
using System.Collections.Generic;
using System.Net.Mail;
using KlasePodataka;
using Repozitorijumi;
using System.Threading.Tasks;
using Servisi;

namespace PoslovnaLogika
{
    public class PrimedbaNaRangListuServis
    {
        private readonly
            IPrimedbaNaRangListuRepository _repo;

        public PrimedbaNaRangListuServis(
            IPrimedbaNaRangListuRepository repo)
        {
            if (repo == null)
            {
                throw new ArgumentNullException("repo");
            }

            _repo = repo;
        }

        public async Task DodajAsync(
            PrimedbaNaRangListuKlasa primedba,
            bool listaJeObjavljena,
            IOgranicenjaKlijent ogranicenjaKlijent)
        {
            if (primedba == null)
            {
                throw new ArgumentNullException(nameof(primedba));
            }

            if (ogranicenjaKlijent == null)
            {
                throw new ArgumentNullException(nameof(ogranicenjaKlijent));
            }

            primedba.Ime = SrediObaveznuVrednost(
                primedba.Ime,
                "Ime",
                100);

            primedba.Prezime = SrediObaveznuVrednost(
                primedba.Prezime,
                "Prezime",
                100);

            primedba.Email = SrediObaveznuVrednost(
                primedba.Email,
                "Email",
                255);

            primedba.Komentar = SrediObaveznuVrednost(
                primedba.Komentar,
                "Komentar",
                2000);

            if (primedba.Komentar.Length < 10)
            {
                throw new ArgumentException(
                    "Komentar mora imati najmanje 10 karaktera.");
            }

            if (!EmailJeIspravan(primedba.Email))
            {
                throw new ArgumentException(
                    "Unesite ispravnu email adresu.");
            }

            var ogranicenja =
                await ogranicenjaKlijent
                    .DajOgranicenjaAsync()
                    .ConfigureAwait(false);

            var pravila = new OgranicenjaKonkursaPravila();

            pravila.ProveriPodnosenjePrimedbe(
                ogranicenja,
                listaJeObjavljena,
                DateTimeOffset.UtcNow);

            // Upis tek nakon uspešne validacije i provere pravila.
            _repo.Dodaj(primedba);
        }

        public List<PrimedbaNaRangListuKlasa>
            DajSve()
        {
            return _repo.DajSve();
        }

        public void OznaciObradjenom(
            int primedbaID,
            int obradioKorisnikID)
        {
            if (primedbaID <= 0)
            {
                throw new ArgumentException(
                    "Neispravan ID primedbe.");
            }

            if (obradioKorisnikID <= 0)
            {
                throw new ArgumentException(
                    "Nije moguće utvrditi administratora.");
            }

            _repo.OznaciObradjenom(
                primedbaID,
                obradioKorisnikID);
        }

        private string SrediObaveznuVrednost(
            string vrednost,
            string nazivPolja,
            int maksimalnaDuzina)
        {
            string sredjenaVrednost =
                string.IsNullOrWhiteSpace(vrednost)
                    ? null
                    : vrednost.Trim();

            if (sredjenaVrednost == null)
            {
                throw new ArgumentException(
                    nazivPolja + " je obavezno.");
            }

            if (sredjenaVrednost.Length >
                maksimalnaDuzina)
            {
                throw new ArgumentException(
                    nazivPolja +
                    " može imati najviše " +
                    maksimalnaDuzina +
                    " karaktera.");
            }

            return sredjenaVrednost;
        }

        private bool EmailJeIspravan(
            string email)
        {
            try
            {
                MailAddress adresa =
                    new MailAddress(email);

                return string.Equals(
                    adresa.Address,
                    email,
                    StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }
    }
}