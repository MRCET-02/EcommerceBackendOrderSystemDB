using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceBackendOrderSystem.Application.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user, IEnumerable<string> roles, out DateTime expiration);
        


    }
}
