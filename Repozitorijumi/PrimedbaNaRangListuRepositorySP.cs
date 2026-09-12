using System.Collections.Generic;
using KlasePodataka;

namespace Repozitorijumi
{
    public class PrimedbaNaRangListuRepositorySP
        : IPrimedbaNaRangListuRepository
    {
        private readonly string _stringKonekcije;

        public PrimedbaNaRangListuRepositorySP(
            string stringKonekcije)
        {
            _stringKonekcije = stringKonekcije;
        }

        public void Dodaj(
            PrimedbaNaRangListuKlasa primedba)
        {
            SPPrimedbaNaRangListuDBKlasa db =
                new SPPrimedbaNaRangListuDBKlasa(
                    _stringKonekcije);

            db.DodajPrimedbu(
                primedba);
        }

        public List<PrimedbaNaRangListuKlasa>
            DajSve()
        {
            SPPrimedbaNaRangListuDBKlasa db =
                new SPPrimedbaNaRangListuDBKlasa(
                    _stringKonekcije);

            return db.DajPrimedbe();
        }

        public void OznaciObradjenom(
            int primedbaID,
            int obradioKorisnikID)
        {
            SPPrimedbaNaRangListuDBKlasa db =
                new SPPrimedbaNaRangListuDBKlasa(
                    _stringKonekcije);

            db.OznaciObradjenom(
                primedbaID,
                obradioKorisnikID);
        }
    }
}