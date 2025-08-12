using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceBackendOrderSystem.Domain.Entities;
using EcommerceBackendOrderSystem.Domain.Enums;

namespace EcommerceBackendOrderSystem.Infrastructure.Repositories.IRepositories
{
    public interface IAdminRepository
    {
        Task<List<string>> GetUserRolesAsync(int userId);
        Task<User?> FindUserExistAsync(string email);
        Task<Role?> GetRoleByNameAsync(RoleType roleType);
        Task<bool> AddUserRoleAsync(UserRole userRole);
    }
}