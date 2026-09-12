using System.Collections.Generic;
using KlasePodataka;

namespace Repozitorijumi
{
    public interface IPreliminarnaRangListaRepository
    {
        void Objavi(
            List<PreliminarnaRangListaKlasa> stavke,
            int objavioKorisnikID);

        List<PreliminarnaRangListaKlasa>
            DajObjavljenuListu();
    }
}