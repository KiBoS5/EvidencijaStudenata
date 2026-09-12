using System.ComponentModel.DataAnnotations;
using KlasePodataka;

namespace KorisnickiInterfejsMVC.Models
{
    public class DodajKorisnikaVM
    {
        [Required(ErrorMessage = "Ime je obavezno")]
        [StringLength(
            100,
            ErrorMessage = "Ime može imati najviše 100 karaktera")]
        public string Ime { get; set; }

        [Required(ErrorMessage = "Prezime je obavezno")]
        [StringLength(
            100,
            ErrorMessage = "Prezime može imati najviše 100 karaktera")]
        public string Prezime { get; set; }

        [Required(ErrorMessage = "Korisničko ime je obavezno")]
        [StringLength(
            50,
            MinimumLength = 3,
            ErrorMessage =
                "Korisničko ime mora imati između 3 i 50 karaktera")]
        public string KorisnickoIme { get; set; }

        [Required(ErrorMessage = "Lozinka je obavezna")]
        [StringLength(
            128,
            MinimumLength = 8,
            ErrorMessage =
                "Lozinka mora imati između 8 i 128 karaktera")]
        [DataType(DataType.Password)]
        public string Lozinka { get; set; }

        [Required(ErrorMessage = "Potvrda lozinke je obavezna")]
        [DataType(DataType.Password)]
        [Compare(
            "Lozinka",
            ErrorMessage = "Lozinka i potvrda lozinke se ne podudaraju")]
        public string PotvrdaLozinke { get; set; }

        [Required(ErrorMessage = "Tip korisnika je obavezan")]
        public TipKorisnika? TipKorisnika { get; set; }
    }
}