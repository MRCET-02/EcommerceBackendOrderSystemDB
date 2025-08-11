using EcommerceBackendOrderSystem.Infrastructure.Repositories;
using EcommerceBackendOrderSystem.Domain.Entities;
using EcommerceBackendOrderSystem.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceBackendOrderSystem.Application.Services
{
    public class ProductService : IProductService

    {
        private readonly IProductRepository _productRepo;
        public ProductService(IProductRepository productRepo) => _productRepo = productRepo;

        public async Task<ProductDto> CreateAsync(ProductDto dto)
        {
            var entity = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                Category = dto.Category
            };

            await _productRepo.AddAsync(entity);
            await _productRepo.SaveChangesAsync();

            dto.Id = entity.Id;
            return dto;
        }

        public async Task DeleteAsync(int id)
        {
            await _productRepo.DeleteAsync(id);
            await _productRepo.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var list = await _productRepo.GetAllAsync();
            return list.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                Category = p.Category
            });
        }

        public async Task<IEnumerable<ProductDto>> GetAvailableAsync()
        {
            var list = await _productRepo.GetAllAvailableAsync();
            return list.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                Category = p.Category
            });
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var p = await _productRepo.GetByIdAsync(id);
            if (p == null) return null;
            return new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                Category = p.Category
            };
        }

        public async Task UpdateAsync(int id, ProductDto dto)
        {
            var existing = await _productRepo.GetByIdAsync(id);
            if (existing == null) throw new KeyNotFoundException("Product not found");

            // update fields
            existing.Name = dto.Name;
            existing.Price = dto.Price;
            existing.StockQuantity = dto.StockQuantity;
            existing.Category = dto.Category;

            await _productRepo.UpdateAsync(existing);
            await _productRepo.SaveChangesAsync();
        }
    }
}