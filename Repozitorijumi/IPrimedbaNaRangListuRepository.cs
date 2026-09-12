using System.Collections.Generic;
using KlasePodataka;

namespace Repozitorijumi
{
    public interface IPrimedbaNaRangListuRepository
    {
        void Dodaj(
            PrimedbaNaRangListuKlasa primedba);

        List<PrimedbaNaRangListuKlasa>
            DajSve();

        void OznaciObradjenom(
            int primedbaID,
            int obradioKorisnikID);
    }
}