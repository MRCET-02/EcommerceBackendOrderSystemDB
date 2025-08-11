using EcommerceBackendOrderSystem.Application.DTO;

using System.Threading.Tasks;

namespace EcommerceBackendOrderSystem.Application.Interfaces
{
    public interface IAdminServices
    {
        Task<bool> AssignRole(AssignroleDTO assignDto);
    }
}
