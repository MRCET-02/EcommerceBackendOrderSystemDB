using EcommerceBackendOrderSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EcommerceBackendOrderSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "DeliveryAgent")]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;
        private readonly UserManager<User> _userManager;

        public DeliveryController(IDeliveryService deliveryService, UserManager<User> userManager)
        {
            _deliveryService = deliveryService;
            _userManager = userManager;
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetAssignedOrders()
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var deliveryAgentId))
                return Unauthorized("Invalid or missing delivery agent ID in token.");

            var orders = await _deliveryService.GetAssignedOrdersAsync(deliveryAgentId);
            return Ok(orders);
        }

        [HttpPut("orders/{orderId}/delivered")]
        public async Task<IActionResult> MarkAsDelivered(int orderId)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var deliveryAgentId))
                return Unauthorized("Invalid or missing delivery agent ID in token.");

            var result = await _deliveryService.MarkOrderAsDeliveredAsync(orderId, deliveryAgentId);
            if (!result)
                return BadRequest("Order not found or not assigned to you.");

            return Ok("Order marked as delivered successfully.");
        }
    }

}
