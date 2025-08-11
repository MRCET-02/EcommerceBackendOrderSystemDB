using EcommerceBackendOrderSystem.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EcommerceBackendOrderSystem.Domain.Enums;

public class Role
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    
    public int Id { get; set; }
    public RoleType Name { get; set; }
    public ICollection<UserRole?> UserRoles { get; set; } 
}
