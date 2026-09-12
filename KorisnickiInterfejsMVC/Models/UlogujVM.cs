using System.ComponentModel.DataAnnotations;

namespace KorisnickiInterfejsMVC.Models
{
    public class UlogujVM
    {
        [Required(
            ErrorMessage = "Korisničko ime je obavezno")]
        [StringLength(
            50,
            ErrorMessage =
                "Korisničko ime može imati najviše 50 karaktera")]
        public string KorisnickoIme { get; set; }

        [Required(
            ErrorMessage = "Lozinka je obavezna")]
        [StringLength(
            128,
            MinimumLength = 8,
            ErrorMessage =
                "Lozinka mora imati između 8 i 128 karaktera")]
        [DataType(DataType.Password)]
        public string Lozinka { get; set; }
    }
}