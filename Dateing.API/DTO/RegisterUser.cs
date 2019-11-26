using System;
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
        [Required]
        public string Gender { get; set; }
        [Required]
        public string KnownAs { get; set; }
        [Required]
        public DateTime DateOfBirtth { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }

        public RegisterUser()
        {
            Created = DateTime.Now;
            LastActive = DateTime.Now;
        }
    }
}