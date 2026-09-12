using System;
using System.IO;
using Newtonsoft.Json;

namespace Servisi
{
    public class OgranicenjaServis
    {
        private OgranicenjaKonkursaKlasa Ucitaj()
        {
            System.Reflection.Assembly assembly =
                typeof(OgranicenjaServis).Assembly;

            string nazivResursa =
                Array.Find(
                    assembly.GetManifestResourceNames(),
                    naziv =>
                        naziv.EndsWith(
                            ".ogranicenja.json",
                            StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrWhiteSpace(nazivResursa))
            {
                throw new FileNotFoundException(
                    "Ugrađeni fajl ogranicenja.json nije pronađen.");
            }

            string json;

            using (Stream stream =
                assembly.GetManifestResourceStream(
                    nazivResursa))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException(
                        "Nije moguće otvoriti ogranicenja.json.");
                }

                using (StreamReader reader =
                    new StreamReader(stream))
                {
                    json = reader.ReadToEnd();
                }
            }

            OgranicenjaKonkursaKlasa ogranicenja =
                JsonConvert.DeserializeObject
                    <OgranicenjaKonkursaKlasa>(json);

            if (ogranicenja == null)
            {
                throw new InvalidOperationException(
                    "Fajl sa ograničenjima nije ispravan.");
            }

            Validiraj(ogranicenja);

            return ogranicenja;
        }

        public OgranicenjaKonkursaKlasa
            DajOgranicenja()
        {
            return Ucitaj();
        }

        public bool KonkursJeOtvoren()
        {
            OgranicenjaKonkursaKlasa ogranicenja =
                Ucitaj();

            DateTimeOffset sada =
                DateTimeOffset.UtcNow;

            return sada >=
                       ogranicenja.DatumPocetkaKonkursa &&
                   sada <=
                       ogranicenja.DatumZavrsetkaKonkursa;
        }

        public bool PrimedbeSuDozvoljene()
        {
            OgranicenjaKonkursaKlasa ogranicenja =
                Ucitaj();

            return DateTimeOffset.UtcNow <=
                ogranicenja.RokZaPrimedbe;
        }

        public int DajMaksimalanBrojPrijava()
        {
            return Ucitaj()
                .MaksimalanBrojPrijava;
        }

        private void Validiraj(
            OgranicenjaKonkursaKlasa ogranicenja)
        {
            if (ogranicenja.DatumPocetkaKonkursa ==
                default(DateTimeOffset))
            {
                throw new InvalidOperationException(
                    "Datum početka konkursa nije podešen.");
            }

            if (ogranicenja.DatumZavrsetkaKonkursa <=
                ogranicenja.DatumPocetkaKonkursa)
            {
                throw new InvalidOperationException(
                    "Datum završetka mora biti posle datuma početka.");
            }

            if (ogranicenja.MaksimalanBrojPrijava <= 0)
            {
                throw new InvalidOperationException(
                    "Maksimalan broj prijava mora biti veći od nule.");
            }

            if (ogranicenja.RokZaPrimedbe <=
                ogranicenja.DatumZavrsetkaKonkursa)
            {
                throw new InvalidOperationException(
                    "Rok za primedbe mora biti posle završetka konkursa.");
            }
        }
    }
}