using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceBackendOrderSystem.Application.DTO;
using EcommerceBackendOrderSystem.Application.Interfaces;
using EcommerceBackendOrderSystem.Domain.Entities;
using EcommerceBackendOrderSystem.Domain.Enums;
using EcommerceBackendOrderSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EcommerceBackendOrderSystem.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OrderResponse> CreateOrderAsync(OrderRequest request)
        {
            var customer = await _context.Users.FindAsync(request.CustomerId);
            if (customer == null)
                throw new Exception("Customer not found.");

            var order = new Order
            {
                CustomerId = request.CustomerId,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                PaymentStatus = PaymentStatus.Pending,
                OrderItems = new List<OrderItem>()
            };

            foreach (var item in request.OrderItems)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null)
                    throw new Exception($"Product ID {item.ProductId} not found.");

                if (product.StockQuantity < item.Quantity)
                    throw new Exception($"Not enough stock for product: {product.Name}");

                product.StockQuantity -= item.Quantity;

                order.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = product.Price
                });
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return await GetOrderByIdAsync(order.Id);
        }

        public async Task<List<OrderResponse>> GetAllOrdersAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();

            return orders.Select(MapOrderToResponse).ToList();
        }

        public async Task<OrderResponse> GetOrderByIdAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                throw new Exception("Order not found.");

            return MapOrderToResponse(order);
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                throw new Exception("Order not found.");

            order.Status = newStatus;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AssignDeliveryAgentAsync(int orderId, int deliveryAgentId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                throw new Exception("Order not found.");

            var deliveryAgent = await _context.Users.FindAsync(deliveryAgentId);
            if (deliveryAgent == null)
                throw new Exception("Delivery agent not found.");

            order.DeliveryAgentId = deliveryAgentId;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return true;
        }

        private OrderResponse MapOrderToResponse(Order order) => new OrderResponse
        {
            OrderId = order.Id,
            CustomerId = order.CustomerId ?? 0,
            OrderDate = order.OrderDate,
            Status = order.Status,
            PaymentStatus = order.PaymentStatus,
            OrderItems = order.OrderItems.Select(oi => new OrderItemDto
            {
                ProductId = oi.ProductId,
                ProductName = oi.Product.Name,
                Quantity = oi.Quantity,
                Price = oi.Price
            }).ToList()
        };
    }
}
