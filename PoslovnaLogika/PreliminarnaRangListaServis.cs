using System;
using System.Collections.Generic;
using System.Linq;
using KlasePodataka;
using Repozitorijumi;

namespace PoslovnaLogika
{
    public class PreliminarnaRangListaServis
    {
        private readonly
            IPreliminarnaRangListaRepository _repo;

        public PreliminarnaRangListaServis(
            IPreliminarnaRangListaRepository repo)
        {
            if (repo == null)
            {
                throw new ArgumentNullException("repo");
            }

            _repo = repo;
        }

        public void Objavi(
            List<PreliminarnaRangListaKlasa> stavke,
            int objavioKorisnikID)
        {
            if (objavioKorisnikID <= 0)
            {
                throw new ArgumentException(
                    "Nije moguće utvrditi administratora.");
            }

            if (stavke == null || stavke.Count == 0)
            {
                throw new ArgumentException(
                    "Nije moguće objaviti praznu rang-listu.");
            }

            if (stavke.Any(x => x.StudentID <= 0))
            {
                throw new ArgumentException(
                    "Rang-lista sadrži neispravnog studenta.");
            }

            if (stavke.Any(x =>
                string.IsNullOrWhiteSpace(x.Ime) ||
                string.IsNullOrWhiteSpace(x.Prezime) ||
                string.IsNullOrWhiteSpace(x.BrojIndeksa)))
            {
                throw new ArgumentException(
                    "Ime, prezime i broj indeksa su obavezni.");
            }

            if (stavke
                .GroupBy(x => x.StudentID)
                .Any(grupa => grupa.Count() > 1))
            {
                throw new ArgumentException(
                    "Student se pojavljuje više puta na rang-listi.");
            }

            for (int i = 0; i < stavke.Count; i++)
            {
                stavke[i].Pozicija = i + 1;

                stavke[i].Ime =
                    stavke[i].Ime.Trim();

                stavke[i].Prezime =
                    stavke[i].Prezime.Trim();

                stavke[i].BrojIndeksa =
                    stavke[i].BrojIndeksa.Trim();
            }

            _repo.Objavi(
                stavke,
                objavioKorisnikID);
        }

       
    }
}