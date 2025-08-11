using System.ComponentModel.DataAnnotations;

namespace EcommerceBackendOrderSystem.Application.DTO
{
    public class UserDTO
    {

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

    }
}
