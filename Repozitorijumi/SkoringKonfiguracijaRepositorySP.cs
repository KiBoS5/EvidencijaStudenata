using KlasePodataka;

namespace Repozitorijumi
{
    public class SkoringKonfiguracijaRepositorySP
        : ISkoringKonfiguracijaRepository
    {
        private readonly string _stringKonekcije;

        public SkoringKonfiguracijaRepositorySP(
            string stringKonekcije)
        {
            _stringKonekcije = stringKonekcije;
        }

        public SkoringKonfiguracijaKlasa DajKonfiguraciju()
        {
            var db = new SPSkoringKonfiguracijaDBKlasa(
                _stringKonekcije);

            return db.DajKonfiguraciju();
        }

        public void SacuvajKonfiguraciju(
            SkoringKonfiguracijaKlasa konfiguracija)
        {
            var db = new SPSkoringKonfiguracijaDBKlasa(
                _stringKonekcije);

            db.SacuvajKonfiguraciju(konfiguracija);
        }
    }
}