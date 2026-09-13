using System;
using System.IO;
using Newtonsoft.Json;

namespace Servisi
{
    public class OgranicenjaServis
    {
        public OgranicenjaKonkursaKlasa DajOgranicenja()
        {
            var assembly = typeof(OgranicenjaServis).Assembly;

            string nazivResursa = Array.Find(
                assembly.GetManifestResourceNames(),
                naziv => naziv.EndsWith(
                    ".ogranicenja.json",
                    StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrWhiteSpace(nazivResursa))
            {
                throw new FileNotFoundException(
                    "Ugrađeni fajl ogranicenja.json nije pronađen.");
            }

            string json;

            using (Stream stream =
                assembly.GetManifestResourceStream(nazivResursa))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException(
                        "Nije moguće otvoriti ogranicenja.json.");
                }

                using (var reader = new StreamReader(stream))
                {
                    json = reader.ReadToEnd();
                }
            }

            var ogranicenja =
                JsonConvert.DeserializeObject
                    <OgranicenjaKonkursaKlasa>(json);

            if (ogranicenja == null)
            {
                throw new InvalidOperationException(
                    "Fajl sa parametrima konkursa je prazan " +
                    "ili ne sadrži objekat konfiguracije.");
            }

            return ogranicenja;
        }
    }
}