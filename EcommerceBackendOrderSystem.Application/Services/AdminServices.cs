using EcommerceBackendOrderSystem.Application.DTO;

using EcommerceBackendOrderSystem.Application.Interfaces;
using EcommerceBackendOrderSystem.Domain.Entities;
using EcommerceBackendOrderSystem.Domain.Enums;
using EcommerceBackendOrderSystem.Infrastructure.Repositories.IRepositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceBackendOrderSystem.Application.Services
{
    public class AdminServices : IAdminServices
    {
        private readonly IAdminRepository _adminRepository;

        public AdminServices(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository ?? throw new ArgumentNullException(nameof(adminRepository));
        }

        public async Task<bool> AssignRole(AssignroleDTO assignDto)
        {
            if (assignDto == null) throw new ArgumentNullException(nameof(assignDto));

            var user = await _adminRepository.FindUserExistAsync(assignDto.Email);
            if (user == null) return false;

            // Parse RoleType enum from input RoleName
            if (!Enum.TryParse<RoleType>(assignDto.RoleName, true, out var roleType))
                return false;

            int roleId = (int)roleType;

            // Get role by Id, since Role.Name is stored as enum int in DB
         

            // Check if user already has the role by RoleId
            if (user.UserRoles.Any(ur => ur.RoleId == roleId))
                return false;

            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = roleId
            };

            await _adminRepository.AddUserRoleAsync(userRole);
            return true;
        }


    }
}