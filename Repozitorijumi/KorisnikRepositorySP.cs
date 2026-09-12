using System.Collections.Generic;
using KlasePodataka;

namespace Repozitorijumi
{
    public class KorisnikRepositorySP : IKorisnikRepository
    {
        private readonly string _stringKonekcije;

        public KorisnikRepositorySP(string stringKonekcije)
        {
            _stringKonekcije = stringKonekcije;
        }

        public int DodajKorisnika(KorisnikKlasa korisnik)
        {
            SPKorisnikDBKlasa db =
                new SPKorisnikDBKlasa(_stringKonekcije);

            return db.DodajKorisnika(korisnik);
        }

        public KorisnikKlasa DajPoKorisnickomImenu(
            string korisnickoIme)
        {
            SPKorisnikDBKlasa db =
                new SPKorisnikDBKlasa(_stringKonekcije);

            return db.DajPoKorisnickomImenu(korisnickoIme);
        }

        public List<KorisnikKlasa> DajSveKorisnike()
        {
            SPKorisnikDBKlasa db =
                new SPKorisnikDBKlasa(_stringKonekcije);

            return db.DajSveKorisnike();
        }

        public void AzurirajDatumPoslednjePrijave(
            int korisnikID)
        {
            SPKorisnikDBKlasa db =
                new SPKorisnikDBKlasa(_stringKonekcije);

            db.AzurirajDatumPoslednjePrijave(korisnikID);
        }
    }
}