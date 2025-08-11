using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceBackendOrderSystem.Infrastructure.Repositories.IRepositories
{
    public interface IAuthentication
    {
        Task<User> FindUserExistAsync(string username);
        Task<List<string>> GetUserRolesAsync(int userId);
    }

}
