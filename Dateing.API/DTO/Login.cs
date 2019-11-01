using System.ComponentModel.DataAnnotations;

namespace Dateing.API.DTO
{
    public class Login
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}