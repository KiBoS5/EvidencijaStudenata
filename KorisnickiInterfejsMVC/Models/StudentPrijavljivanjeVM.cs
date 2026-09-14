using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace KorisnickiInterfejsMVC.Models
{
    public class StudentPrijavljivanjeVM
    {
        [Required(ErrorMessage = "Ime je obavezno")]
        public string Ime { get; set; }

        [Required(ErrorMessage = "Prezime je obavezno")]
        public string Prezime { get; set; }

        [Required(ErrorMessage = "Datum rođenja je obavezan")]
        [DataType(DataType.Date)]
        public DateTime DatumRodjenja { get; set; }

        [Required(ErrorMessage = "Email je obavezan")]
        [EmailAddress(ErrorMessage = "Neispravan email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefon je obavezan")]
        public string Telefon { get; set; }

        [Required(ErrorMessage = "Broj indeksa je obavezan")]
        public string BrojIndeksa { get; set; }

        [Required(ErrorMessage = "Studijski program je obavezan")]
        public string StudijskiProgram { get; set; }

        [Required(ErrorMessage = "Godina studija je obavezna")]
        public string GodinaStudija { get; set; }

        public bool BezRoditelja { get; set; }

        [Required(ErrorMessage = "Prosek je obavezan")]
        [Range(6.00, 10.00, ErrorMessage = "Prosek mora biti između 6.00 i 10.00")]
        public decimal Prosek { get; set; }

        public bool DokumentacijaPrihodaDostavljena { get; set; }

        [Required(ErrorMessage = "Ukupna primanja domaćinstva su obavezna")]
        [Range(0, 10000000, ErrorMessage = "Unesite validan iznos primanja")]
        public decimal UkupnaPrimanjaDomacinstva { get; set; }

        [Required(ErrorMessage = "Broj članova porodice je obavezan")]
        [Range(1, 20, ErrorMessage = "Broj članova porodice mora biti između 1 i 20")]
        public int BrojClanovaPorodice { get; set; }

        public bool DokumentacijaPrihodaDostavljenaChecked { get; set; }
    }
}
