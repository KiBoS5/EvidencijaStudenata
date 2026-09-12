using System.Collections.Generic;
using KlasePodataka;

namespace Repozitorijumi
{
    public class PreliminarnaRangListaRepositorySP
        : IPreliminarnaRangListaRepository
    {
        private readonly string _stringKonekcije;

        public PreliminarnaRangListaRepositorySP(
            string stringKonekcije)
        {
            _stringKonekcije = stringKonekcije;
        }

        public void Objavi(
            List<PreliminarnaRangListaKlasa> stavke,
            int objavioKorisnikID)
        {
            SPPreliminarnaRangListaDBKlasa db =
                new SPPreliminarnaRangListaDBKlasa(
                    _stringKonekcije);

            db.ObjaviPreliminarnuRangListu(
                stavke,
                objavioKorisnikID);
        }

        public List<PreliminarnaRangListaKlasa>
            DajObjavljenuListu()
        {
            SPPreliminarnaRangListaDBKlasa db =
                new SPPreliminarnaRangListaDBKlasa(
                    _stringKonekcije);

            return db.DajPreliminarnuRangListu();
        }
    }
}