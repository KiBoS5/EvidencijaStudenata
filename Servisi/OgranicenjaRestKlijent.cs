using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Servisi
{
    public class OgranicenjaRestKlijent : IOgranicenjaKlijent
    {
        private static readonly HttpClient Klijent =
            new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(15)
            };

        private readonly Uri _adresa;

        public OgranicenjaRestKlijent(string adresa)
        {
            Uri proverenaAdresa;

            if (!Uri.TryCreate(
                    adresa,
                    UriKind.Absolute,
                    out proverenaAdresa) ||
                (proverenaAdresa.Scheme != Uri.UriSchemeHttp &&
                 proverenaAdresa.Scheme != Uri.UriSchemeHttps))
            {
                throw new ArgumentException(
                    "Adresa servisa za ograničenja nije ispravna.",
                    nameof(adresa));
            }

            _adresa = proverenaAdresa;
        }

        public async Task<OgranicenjaKonkursaKlasa>
            DajOgranicenjaAsync()
        {
            using (HttpResponseMessage odgovor =
                await Klijent.GetAsync(_adresa)
                    .ConfigureAwait(false))
            {
                odgovor.EnsureSuccessStatusCode();

                string json =
                    await odgovor.Content.ReadAsStringAsync()
                        .ConfigureAwait(false);

                var ogranicenja =
                    JsonConvert.DeserializeObject
                        <OgranicenjaKonkursaKlasa>(json);

                if (ogranicenja == null)
                {
                    throw new InvalidOperationException(
                        "Servis nije vratio parametre konkursa.");
                }

                return ogranicenja;
            }
        }
    }
}