using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EcommerceBackendOrderSystem.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace EcommerceBackendOrderSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "DeliveryAgent")]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;

        public DeliveryController(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetOrders()
        {
            var deliveryAgentId = int.Parse(User.FindFirst("sub").Value);
            var orders = await _deliveryService.GetAssignedOrdersAsync(deliveryAgentId);
            return Ok(orders);
        }

        [HttpPut("orders/{orderId}/delivered")]
        public async Task<IActionResult> MarkAsDelivered(int orderId)
        {
            var deliveryAgentId = int.Parse(User.FindFirst("sub").Value);
            var result = await _deliveryService.MarkOrderAsDeliveredAsync(orderId, deliveryAgentId);
            if (!result) return BadRequest("Order not found or not assigned to you.");
            return Ok("Order marked as delivered.");
        }
    }
}