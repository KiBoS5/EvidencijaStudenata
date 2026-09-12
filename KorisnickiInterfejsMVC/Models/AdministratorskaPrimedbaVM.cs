using System;

namespace KorisnickiInterfejsMVC.Models
{
    public class AdministratorskaPrimedbaVM
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

        public string ObradioImeIPrezime
        {
            get;
            set;
        }

        public string ObradioKorisnickoIme
        {
            get;
            set;
        }
    }
}