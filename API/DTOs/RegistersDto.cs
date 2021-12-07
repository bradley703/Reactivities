using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{   
    
    public class RegistersDto
    {   [Required]
        public string DisplayName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$", ErrorMessage = "Password must be strong")]
        public string Password { get; set; }
        public string Username { get; set; }
    }
}