using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceBackendOrderSystem.Application.DTO;

using EcommerceBackendOrderSystem.Application.Interfaces;
using EcommerceBackendOrderSystem.Domain.Entities;
using EcommerceBackendOrderSystem.Infrastructure.Data;


namespace EcommerceBackendOrderSystem.Application.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly ApplicationDbContext _context;

        public DeliveryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderDto>> GetAssignedOrdersAsync(int deliveryAgentId)
        {
            var orders = _context.Orders
                .Where(o => o.DeliveryAgentId == deliveryAgentId)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    Status = o.Status.ToString(),
                    DeliveryStatus = o.Status
                }).ToList();

            return await Task.FromResult(orders);
        }

        public async Task<bool> MarkOrderAsDeliveredAsync(int orderId, int deliveryAgentId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId && o.DeliveryAgentId == deliveryAgentId);
            if (order == null) return false;

            order.Status = Domain.Enums.OrderStatus.Delivered;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}