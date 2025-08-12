using EcommerceBackendOrderSystem.Application.DTO;
using EcommerceBackendOrderSystem.Application.Interfaces;
using EcommerceBackendOrderSystem.Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventTicketingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userServices;
        private readonly IAuthService _authService;
        private readonly IAdminServices _adminServices;

        public AccountController(IUserService userServices, IAuthService authService, IAdminServices adminServices)
        {
            _userServices = userServices;
            _authService = authService;
            _adminServices = adminServices;
        }

        [HttpPost("UserRegister")]
        public async Task<IActionResult> RegisterUser([FromBody] UserDTO userDTO)
        {
            if (userDTO == null)
                return BadRequest(new { message = "User Details cannot be empty" });

            if (!userDTO.Email.EndsWith("@gmail.com"))
                return BadRequest(new { message = "Only Gmail addresses are allowed .ex(user@gmail.com)." });

            bool isUser = await _userServices.RegisterUserAsync(userDTO);
            if (!isUser)
                return BadRequest(new { message = "User Already Exist! Try Logging in" });

            return Ok(new { message = "User Details Entered Successfully" });
        }

        [HttpPost("UserLogin")]
        public async Task<IActionResult> LoginUserAsync([FromBody] LoginDTO loginDTO)
        {
            if (loginDTO == null)
                return BadRequest(new { message = "User credentials cannot be null" });

            string? token = await _authService.GenerateJwtAsync(loginDTO);
            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Enter Valid Credentials" });

            return Ok(new { token = token });
        }

        [HttpPost("AssignRole")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRole([FromBody] AssignroleDTO assignDto)
        {
            if (assignDto == null)
                return BadRequest(new { message = "Role details cannot be null" });

            // Get logged-in user's email from JWT claims
            var currentUserEmail = User.Identity?.Name;
            if (string.IsNullOrEmpty(currentUserEmail))
                return Unauthorized(new { message = "User identity not found in token" });

            try
            {
                bool result = await _adminServices.AssignRole(assignDto);
                if (!result)
                    return BadRequest(new { message = "Failed to assign role. Check if user and role exist or role already assigned." });

                return Ok(new { message = $"Role '{assignDto.RoleName}' assigned to user {assignDto.Email}" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message); // "Only admins can assign roles."
            }
        }

    }
}
