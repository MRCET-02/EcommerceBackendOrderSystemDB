using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthentication_Web_Api_Session.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class SecureController : Controller
    {
        [HttpGet("PinMeWithSecureJwtToken")]
        public IActionResult Index()
        {
            return Ok("you are authoirzed as admin !");
        }
    }
}
