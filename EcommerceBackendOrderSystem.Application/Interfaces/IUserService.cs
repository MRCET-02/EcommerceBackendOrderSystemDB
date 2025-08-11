
using System.Threading.Tasks;
using EcommerceBackendOrderSystem.Application.DTO;
namespace EcommerceBackendOrderSystem.Application.Interfaces
{
    
        public interface IUserService
        {
            Task<bool> RegisterUserAsync(UserDTO userDTO);
        }

    
}
