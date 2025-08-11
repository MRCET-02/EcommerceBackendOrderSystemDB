using System.ComponentModel.DataAnnotations;

namespace EcommerceBackendOrderSystem.Application.DTO
{
    public class AssignroleDTO
    {
        [Required]
        public int UserId { get; set; }     

        [Required]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Email { get; set; }

    }
}
