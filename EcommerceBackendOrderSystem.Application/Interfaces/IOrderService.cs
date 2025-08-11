using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcommerceBackendOrderSystem.Application.DTO;

namespace EcommerceBackendOrderSystem.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponse> CreateOrderAsync(OrderRequest request);
        Task<List<OrderResponse>> GetAllOrdersAsync();
        Task<OrderResponse> GetOrderByIdAsync(int orderId);
        Task<bool> UpdateOrderStatusAsync(int orderId, Domain.Enums.OrderStatus newStatus);
    }
}