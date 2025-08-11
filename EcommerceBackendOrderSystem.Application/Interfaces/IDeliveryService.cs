using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceBackendOrderSystem.Application.DTO;

namespace EcommerceBackendOrderSystem.Application.Interfaces
{
    public interface IDeliveryService
    {
        Task<IEnumerable<OrderDto>> GetAssignedOrdersAsync(int deliveryAgentId);
        Task<bool> MarkOrderAsDeliveredAsync(int orderId, int deliveryAgentId);
    }
}