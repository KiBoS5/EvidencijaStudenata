using KlasePodataka;

namespace Repozitorijumi
{
    public interface ISkoringKonfiguracijaRepository
    {
        SkoringKonfiguracijaKlasa DajKonfiguraciju();

        void SacuvajKonfiguraciju(
            SkoringKonfiguracijaKlasa konfiguracija);
    }
}