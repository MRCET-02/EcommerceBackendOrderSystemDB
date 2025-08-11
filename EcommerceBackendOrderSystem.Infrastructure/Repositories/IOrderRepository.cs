using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcommerceBackendOrderSystem.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceBackendOrderSystem.Infrastructure.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> GetByIdAsync(int orderId);
        Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId);
        Task<IEnumerable<Order>> GetAllAsync();
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(Order order);
    }
}