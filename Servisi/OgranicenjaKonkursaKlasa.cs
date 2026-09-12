using System;

namespace Servisi
{
    public class OgranicenjaKonkursaKlasa
    {
        public DateTimeOffset DatumPocetkaKonkursa
        {
            get;
            set;
        }

        public DateTimeOffset DatumZavrsetkaKonkursa
        {
            get;
            set;
        }

        public int MaksimalanBrojPrijava
        {
            get;
            set;
        }

        public DateTimeOffset RokZaPrimedbe
        {
            get;
            set;
        }
    }
}