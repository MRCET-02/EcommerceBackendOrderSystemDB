using EcommerceBackendOrderSystem.Domain.DTO;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EcommerceBackendOrderSystem.Application.Interfaces;
using EcommerceBackendOrderSystem.Infrastructure.Repositories.IRepositories;

namespace EcommerceBackendOrderSystem.Application.Services
{
    public class AuthServices : IAuthService
    {
        private readonly IAuthenticationRepository _authRepo;
        private readonly JwtSettings _jwtSettings;

        public AuthServices(IAuthenticationRepository authRepo, IOptions<JwtSettings> jwtOptions)
        {
            _authRepo = authRepo;
            _jwtSettings = jwtOptions.Value;
        }

        public async Task<string?> GenerateJwtAsync(LoginDTO loginDTO)
        {
            var user = await _authRepo.FindUserExistAsync(loginDTO.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.PasswordHash))
            {
                return null;
            }

            var roles = await _authRepo.GetUserRolesAsync(user.Id);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Sid, user.Id.ToString())
            };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
