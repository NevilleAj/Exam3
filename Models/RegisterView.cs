using System.ComponentModel.DataAnnotations;
namespace exam3.Models
{
    public class RegisterView 
    {
        [Required]
        [MinLength(1)]
        public string UserName { get; set; }
        [Required]
        [MinLength(1)]
        [RegularExpression(@"^[a-zA-Z0-9]+$")]
        public string UserAlias { get; set; }
        [Required]
        [MinLength(1)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLengthAttribute(8)]
        public string Password { get; set; }
        [Compare(nameof(Password))]
        public string PasswordConf { get; set; }
    }
}