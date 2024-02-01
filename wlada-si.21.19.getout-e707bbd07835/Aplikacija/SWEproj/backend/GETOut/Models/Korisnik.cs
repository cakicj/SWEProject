using System.ComponentModel.DataAnnotations;

namespace GETOut.Models
{
    public class Korisnik
    {
        public string Uloga { get; set; }

        public string Id { get; set; }

        public string Token { get; set; }

        public string ValidTo { get; set; }

        [Required(ErrorMessage = "Polje ime je obavezno")]
        public string Ime { get; set; }

        [Required(ErrorMessage = "Polje prezime je obavezno")]
        public string Prezime { get; set; }

        [Required(ErrorMessage = "Polje username je obavezno")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Polje lozinka je obavezno!")]

        public string Password { get; set; }

        [Required(ErrorMessage = "Polje broj telefona je obavezno")]

        public string BrTelefona { get; set; }

        [Required(ErrorMessage = "Polje email je obavezno!")]
        public string Email { get; set; }
    }
}
