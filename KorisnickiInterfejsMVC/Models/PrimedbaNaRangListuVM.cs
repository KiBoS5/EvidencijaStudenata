using System.ComponentModel.DataAnnotations;

namespace KorisnickiInterfejsMVC.Models
{
    public class PrimedbaNaRangListuVM
    {
        [Required(ErrorMessage = "Ime je obavezno.")]
        [StringLength(
            100,
            ErrorMessage =
                "Ime može imati najviše 100 karaktera.")]
        public string Ime { get; set; }

        [Required(ErrorMessage = "Prezime je obavezno.")]
        [StringLength(
            100,
            ErrorMessage =
                "Prezime može imati najviše 100 karaktera.")]
        public string Prezime { get; set; }

        [Required(ErrorMessage = "Email je obavezan.")]
        [EmailAddress(
            ErrorMessage =
                "Unesite ispravnu email adresu.")]
        [StringLength(
            255,
            ErrorMessage =
                "Email može imati najviše 255 karaktera.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Komentar je obavezan.")]
        [StringLength(
            2000,
            MinimumLength = 10,
            ErrorMessage =
                "Komentar mora imati između 10 i 2000 karaktera.")]
        public string Komentar { get; set; }
    }
}