using EcommerceBackendOrderSystem.Domain.Entities;
using EcommerceBackendOrderSystem.Infrastructure.Data;
using EcommerceBackendOrderSystem.Infrastructure.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceBackendOrderSystem.Infrastructure.Repositories.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly ApplicationDbContext _context;

        public AuthenticationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> FindUserExistAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<List<string>> GetUserRolesAsync(int userId)
        {
            return await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.Role.Name.ToString()) // Convert enum to string
                .ToListAsync();
        }
    }
}
