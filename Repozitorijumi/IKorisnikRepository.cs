using System.Collections.Generic;
using KlasePodataka;

namespace Repozitorijumi
{
    public interface IKorisnikRepository
    {
        int DodajKorisnika(KorisnikKlasa korisnik);

        KorisnikKlasa DajPoKorisnickomImenu(
            string korisnickoIme);

        List<KorisnikKlasa> DajSveKorisnike();

        void AzurirajDatumPoslednjePrijave(int korisnikID);
    }
}