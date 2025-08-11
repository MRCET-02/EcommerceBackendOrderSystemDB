using EcommerceBackendOrderSystem.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; }
    public virtual ICollection<Order> Orders { get; set; } 
}
