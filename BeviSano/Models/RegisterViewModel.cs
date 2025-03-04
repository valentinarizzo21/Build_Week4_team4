using System.ComponentModel.DataAnnotations;

namespace BeviSano.Models
{
    public class RegisterViewModel
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Il nome è obbligatorio!")]
        public string Name { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "l'email è obbligatoria!")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "La password è obbligatoria!")]
        public string Password { get; set; }
    }
}
