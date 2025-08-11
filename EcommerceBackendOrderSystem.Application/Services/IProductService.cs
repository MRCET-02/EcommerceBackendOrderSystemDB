using EcommerceBackendOrderSystem.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceBackendOrderSystem.Application.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAvailableAsync();   // visible to customers
        Task<IEnumerable<ProductDto>> GetAllAsync();         // visible to admin only
        Task<ProductDto> GetByIdAsync(int id);
        Task<ProductDto> CreateAsync(ProductDto dto);
        Task UpdateAsync(int id, ProductDto dto);
        Task DeleteAsync(int id);
    }
}

