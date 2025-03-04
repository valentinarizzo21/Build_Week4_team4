using System.ComponentModel.DataAnnotations;

namespace BeviSano.Models
{
    public class LoginViewModel
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Il nome è obbligatorio!")]
        public string Name {  get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "La password è obbligatoria!")]
        public string Password { get; set; }
    }
}
