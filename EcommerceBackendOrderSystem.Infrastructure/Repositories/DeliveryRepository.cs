using System;
using System.Collections.Generic;
using System.Linq;
using EcommerceBackendOrderSystem.Domain.Entities;
using EcommerceBackendOrderSystem.Domain.Enums;
using EcommerceBackendOrderSystem.Infrastructure.Data;

namespace EcommerceBackendOrderSystem.Infrastructure.Repositories
{
    public class DeliveryRepository
    {
        private readonly ApplicationDbContext _context;

        public DeliveryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Order> GetOrdersByAgent(int deliveryAgentId)
        {
            return _context.Orders.Where(o => o.DeliveryAgentId == deliveryAgentId).ToList();
        }

        public bool MarkAsDelivered(int orderId, int deliveryAgentId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId && o.DeliveryAgentId == deliveryAgentId);
            if (order == null) return false;
            order.Status = OrderStatus.Delivered;
            _context.SaveChanges();
            return true;
        }
    }
}
