using EcommerceBackendOrderSystem.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceBackendOrderSystem.Infrastructure.Repositories.IRepositories
{
    public interface IAuthenticationRepository
    {
        Task<User?> FindUserExistAsync(string username);
        Task<List<string>> GetUserRolesAsync(int userId);
    }
}
