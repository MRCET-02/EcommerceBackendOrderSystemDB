using EcommerceBackendOrderSystem.Domain.Entities;
using EcommerceBackendOrderSystem.Domain.Enums;
using EcommerceBackendOrderSystem.Infrastructure.Data;
using EcommerceBackendOrderSystem.Infrastructure.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EcommerceBackendOrderSystem.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) => _context = context;

        public async Task RegisterUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Assuming 'Name' is the enum property in Role for the RoleType
            var customerRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == RoleType.Customer);

            if (customerRole != null)
            {
                var userRole = new UserRole
                {
                    UserId = user.Id,
                    RoleId = customerRole.Id
                };
                _context.UserRoles.Add(userRole);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<User?> FindUserExistAsync(string email)
        {
            return await _context.Users
                .Include(u => u.UserRoles) // Include roles if needed
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<Role?> GetRoleByIdAsync(int roleId)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
        }

        public async Task<Role?> GetRoleByNameAsync(RoleType roleType)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleType);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
