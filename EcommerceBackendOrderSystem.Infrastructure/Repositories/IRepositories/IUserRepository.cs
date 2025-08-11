using EcommerceBackendOrderSystem.Domain.Entities;
using EcommerceBackendOrderSystem.Domain.Enums;
using System.Threading.Tasks;

namespace EcommerceBackendOrderSystem.Infrastructure.Repositories.IRepositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(int userId);
        Task<Role?> GetRoleByIdAsync(int roleId);
        Task<User?> FindUserExistAsync(string email);
        Task RegisterUserAsync(User user);
        Task SaveChangesAsync();

    }
}
