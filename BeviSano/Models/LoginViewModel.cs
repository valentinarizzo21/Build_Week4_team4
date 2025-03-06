using System.ComponentModel.DataAnnotations;

namespace BeviSano.Models
{
    public class LoginViewModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "L'indirizzo email è obbligatorio!")]
        public string Email {  get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "La password è obbligatoria!")]
        public string Password { get; set; }
    }
}
