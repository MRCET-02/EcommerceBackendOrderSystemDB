using EcommerceBackendOrderSystem.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceBackendOrderSystem.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string?> GenerateJwtAsync(LoginDTO loginDTO);
    }
}

