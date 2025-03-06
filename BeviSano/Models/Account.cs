using System.ComponentModel.DataAnnotations;

namespace BeviSano.Models
{
    public class Account
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool admin {  get; set; }
        public bool fidelity { get; set; }
    }
}
