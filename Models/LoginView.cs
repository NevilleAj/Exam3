using System.ComponentModel.DataAnnotations;

namespace exam3.Models
{
    public class LoginView 
    {
        [Required]
        [MinLength(1)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLengthAttribute(8)]
        public string Password { get; set; }
    }
}