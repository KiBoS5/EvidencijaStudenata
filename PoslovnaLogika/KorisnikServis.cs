using System;
using System.Collections.Generic;
using KlasePodataka;
using Microsoft.AspNet.Identity;
using Repozitorijumi;

namespace PoslovnaLogika
{
    public class KorisnikServis
    {
        private readonly IKorisnikRepository _repo;
        private readonly PasswordHasher _passwordHasher;

        public KorisnikServis(IKorisnikRepository repo)
        {
            _repo = repo;
            _passwordHasher = new PasswordHasher();
        }

        public int DodajKorisnika(
            KorisnikKlasa korisnik,
            string lozinka)
        {
            ValidirajNovogKorisnika(korisnik, lozinka);

            korisnik.Ime = korisnik.Ime.Trim();
            korisnik.Prezime = korisnik.Prezime.Trim();
            korisnik.KorisnickoIme =
                korisnik.KorisnickoIme.Trim();

            KorisnikKlasa postojeci =
                _repo.DajPoKorisnickomImenu(
                    korisnik.KorisnickoIme);

            if (postojeci != null)
            {
                throw new InvalidOperationException(
                    "Korisničko ime već postoji.");
            }

            korisnik.LozinkaHash =
                _passwordHasher.HashPassword(lozinka);

            korisnik.Aktivan = true;

            return _repo.DodajKorisnika(korisnik);
        }

        public KorisnikKlasa PrijaviKorisnika(
            string korisnickoIme,
            string lozinka)
        {
            if (string.IsNullOrWhiteSpace(korisnickoIme) ||
                string.IsNullOrEmpty(lozinka))
            {
                return null;
            }

            KorisnikKlasa korisnik =
                _repo.DajPoKorisnickomImenu(
                    korisnickoIme.Trim());

            if (korisnik == null || !korisnik.Aktivan)
            {
                return null;
            }

            PasswordVerificationResult rezultat =
                _passwordHasher.VerifyHashedPassword(
                    korisnik.LozinkaHash,
                    lozinka);

            if (rezultat == PasswordVerificationResult.Failed)
            {
                return null;
            }

            _repo.AzurirajDatumPoslednjePrijave(korisnik.ID);

            // Hash ne treba prosleđivati kontroleru ili Viewu.
            korisnik.LozinkaHash = null;
            korisnik.DatumPoslednjePrijave = DateTime.UtcNow;

            return korisnik;
        }

        public List<KorisnikKlasa> DajSveKorisnike()
        {
            return _repo.DajSveKorisnike();
        }

        private void ValidirajNovogKorisnika(
            KorisnikKlasa korisnik,
            string lozinka)
        {
            if (korisnik == null)
            {
                throw new ArgumentNullException("korisnik");
            }

            if (string.IsNullOrWhiteSpace(korisnik.Ime))
            {
                throw new ArgumentException(
                    "Ime je obavezno.");
            }

            if (string.IsNullOrWhiteSpace(korisnik.Prezime))
            {
                throw new ArgumentException(
                    "Prezime je obavezno.");
            }

            if (string.IsNullOrWhiteSpace(
                    korisnik.KorisnickoIme))
            {
                throw new ArgumentException(
                    "Korisničko ime je obavezno.");
            }

            string korisnickoIme =
                korisnik.KorisnickoIme.Trim();

            if (korisnickoIme.Length < 3 ||
                korisnickoIme.Length > 50)
            {
                throw new ArgumentException(
                    "Korisničko ime mora imati između 3 i 50 karaktera.");
            }

            if (string.IsNullOrEmpty(lozinka) ||
                lozinka.Length < 8 ||
                lozinka.Length > 128)
            {
                throw new ArgumentException(
                    "Lozinka mora imati između 8 i 128 karaktera.");
            }

            if (!Enum.IsDefined(
                    typeof(TipKorisnika),
                    korisnik.TipKorisnika))
            {
                throw new ArgumentException(
                    "Izabran je neispravan tip korisnika.");
            }
        }
    }
}