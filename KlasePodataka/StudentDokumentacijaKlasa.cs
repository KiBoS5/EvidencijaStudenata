using System;

namespace KlasePodataka
{
    public class StudentDokumentacijaKlasa
    {
        public int ID { get; set; }
        public int StudentID { get; set; }

        public bool SviDokumentiPodneseni { get; set; }
        public DateTime? DatumPodnosenja { get; set; }

        public bool SviDokumentiProvereni { get; set; }
        public DateTime? DatumProvere { get; set; }

        public bool? DokumentacijaIspravna { get; set; }
        public string NapomenaProvere { get; set; }

        public bool SpremnaZaUnos { get; set; }

        public DateTime DatumKreiranja { get; set; }
        public DateTime DatumAzuriranja { get; set; }

        public int? ProverioKorisnikID { get; set; }
        public string ProverioIme { get; set; }
        public string ProverioPrezime { get; set; }
        public string ProverioKorisnickoIme { get; set; }

        public string StudentIme { get; set; }
        public string StudentPrezime { get; set; }
        public string BrojIndeksa { get; set; }

        public string ProverioImeIPrezime
        {
            get
            {
                string imeIPrezime =
                    string.Format(
                        "{0} {1}",
                        ProverioIme,
                        ProverioPrezime)
                    .Trim();

                return string.IsNullOrWhiteSpace(imeIPrezime)
                    ? ProverioKorisnickoIme
                    : imeIPrezime;
            }
        }

        public string StanjeProcesa
        {
            get
            {
                if (!SviDokumentiPodneseni)
                {
                    return "Čeka podnošenje";
                }

                if (!SviDokumentiProvereni)
                {
                    return "Čeka proveru";
                }

                if (DokumentacijaIspravna == false)
                {
                    return "Dokumentacija nije ispravna";
                }

                if (DokumentacijaIspravna == true &&
                    SpremnaZaUnos)
                {
                    return "Spremna za unos";
                }

                return "Proverena";
            }
        }
    }
}