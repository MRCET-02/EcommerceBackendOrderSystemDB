using EcommerceBackendOrderSystem.Application.DTO;
using EcommerceBackendOrderSystem.Application.Interfaces;
using EcommerceBackendOrderSystem.Domain.DTO;

using Microsoft.AspNetCore.Mvc;
namespace EventTicketingSystem.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userServices;
        private readonly IAuthService _authService;
        public AccountController(IUserService userServices, IAuthService authService)
        {
            _userServices = userServices;
            _authService = authService;
        }
        [HttpPost("UserRegister")]
        public async Task<IActionResult> RegisterUser([FromBody] UserDTO userDTO)
        {
            if (userDTO == null)
            {
                return BadRequest(new { message = "User Details cannot be empty" });
            }
            if (!userDTO.Email.EndsWith("@gmail.com"))
            {
                return BadRequest(new { message = "Only Gmail addresses are allowed .ex(user@gmail.com)." });
            }

            bool isUser = await _userServices.RegisterUserAsync(userDTO);
            if (!isUser)
            {
                return BadRequest(new { message = "User Already Exist! Try Logging in" });
            }
            else
            {
                return Ok(new { message = "User Details Entered Successfully" });
            }
        }

        [HttpPost("UserLogin")]
        public async Task<IActionResult> LoginUserAsync([FromBody] LoginDTO loginDTO)
        {
            if (loginDTO == null)
            {
                return BadRequest(new { message = "User credentials cannot be null" });
            }
            string? token = await _authService.GenerateJwtAsync(loginDTO);
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { message = "Enter Valid Credentials" });
            }
            else
            {
                return Ok(new { token = token });
            }
        }
    }
}