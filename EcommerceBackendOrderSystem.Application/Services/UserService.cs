using EcommerceBackendOrderSystem.Infrastructure.Repositories.IRepositories;
using EcommerceBackendOrderSystem.Application.Interfaces;
using EcommerceBackendOrderSystem.Application.DTO;
using EcommerceBackendOrderSystem.Domain.Entities;
using EcommerceBackendOrderSystem.Domain.Enums;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceBackendOrderSystem.Application.Services
{
    public class UserServices : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserServices(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // User Registration
        public async Task<bool> RegisterUserAsync(UserDTO userDTO)
        {
            var isAlreadyUser = await _userRepository.FindUserExistAsync(userDTO.Email);
            if (isAlreadyUser != null) return false;

            var newUser = new User
            {
                Email = userDTO.Email,
                Username = userDTO.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDTO.Password),
            };

            await _userRepository.RegisterUserAsync(newUser);
            await _userRepository.SaveChangesAsync();  // <-- This is crucial

            return true;
        }


        // Assign Role (Only Admin can assign DeliveryAgent)
        public async Task<bool> AssignRoleAsync(int adminUserId, AssignroleDTO assignDto)
        {
            // 1. Verify adminUserId belongs to an Admin user
            var adminUser = await _userRepository.GetUserByIdAsync(adminUserId);
            if (adminUser == null || !adminUser.UserRoles.Any(ur => ur.Role.Name == RoleType.Admin))
            {
                return false; // Not an admin
            }

            // 2. Get the Role entity by RoleId from DTO
            var role = await _userRepository.GetRoleByIdAsync(assignDto.RoleId);
            if (role == null || role.Name != RoleType.DeliveryAgent)
            {
                return false; // Only DeliveryAgent role assignment allowed here
            }

            // 3. Get the target user by UserId from DTO
            var targetUser = await _userRepository.GetUserByIdAsync(assignDto.UserId);
            if (targetUser == null)
            {
                return false; // Target user not found
            }

            // 4. Check if target user already has the role
            if (targetUser.UserRoles.Any(ur => ur.RoleId == role.Id))
            {
                return false; // Role already assigned
            }

            // 5. Assign role
            targetUser.UserRoles.Add(new UserRole
            {
                UserId = assignDto.UserId,
                RoleId = role.Id
            });

            await _userRepository.SaveChangesAsync();
            return true;
        }
    }
}
