using System.ComponentModel.DataAnnotations;

namespace Dateing.API.DTO
{
    public class RegisterUser
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "password should be between 4 and 8 charector")]
        public string Password { get; set; }
    }
}