using EcommerceBackendOrderSystem.Application.DTO;

using EcommerceBackendOrderSystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceBackendOrderSystem.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequest request)
        {
            var result = await _orderService.CreateOrderAsync(request);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()   //Gets all orders from orderService
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            return Ok(order);
        }

        [HttpPut("{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromQuery] string newStatus)
        {
            if (!Enum.TryParse<Domain.Enums.OrderStatus>(newStatus, true, out var statusEnum))
                return BadRequest("Invalid status value.");

            await _orderService.UpdateOrderStatusAsync(orderId, statusEnum); //calling service method to update order status in db
            return NoContent();
        }
    }
}