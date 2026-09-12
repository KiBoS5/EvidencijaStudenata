using System;

namespace KlasePodataka
{
    public class PrimedbaNaRangListuKlasa
    {
        public int ID { get; set; }

        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Email { get; set; }
        public string Komentar { get; set; }

        public DateTime DatumPodnosenja { get; set; }

        public bool Obradjena { get; set; }
        public DateTime? DatumObrade { get; set; }

        public int? ObradioKorisnikID { get; set; }
        public string ObradioIme { get; set; }
        public string ObradioPrezime { get; set; }

        public string ObradioKorisnickoIme
        {
            get;
            set;
        }

        public string ObradioImeIPrezime
        {
            get
            {
                string imeIPrezime =
                    string.Format(
                        "{0} {1}",
                        ObradioIme,
                        ObradioPrezime)
                    .Trim();

                return string.IsNullOrWhiteSpace(imeIPrezime)
                    ? ObradioKorisnickoIme
                    : imeIPrezime;
            }
        }
    }
}