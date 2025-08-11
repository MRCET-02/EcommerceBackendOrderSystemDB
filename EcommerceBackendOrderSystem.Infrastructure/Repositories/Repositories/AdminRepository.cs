using EcommerceBackendOrderSystem.Domain.Entities;
using EcommerceBackendOrderSystem.Domain.Enums;
using EcommerceBackendOrderSystem.Infrastructure.Data;
using EcommerceBackendOrderSystem.Infrastructure.Repositories.IRepositories;

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace EcommerceBackendOrderSystem.Infrastructure.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _context;

        public AdminRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> FindUserExistAsync(string email)
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Role?> GetRoleByNameAsync(RoleType roleType)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.Name == roleType);
        }

        public async Task<bool> AddUserRoleAsync(UserRole userRole)
        {
            _context.UserRoles.Add(userRole);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<string>> GetUserRolesAsync(int userId)
        {
            return await _context.UserRoles
                .Where(ur => ur.User.Id == userId)
                .Select(ur => ur.Role.UserRoles.ToString())
                .ToListAsync();
        }
    }
}
