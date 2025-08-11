using EcommerceBackendOrderSystem.Domain.Entities;
using EcommerceBackendOrderSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceBackendOrderSystem.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) => _context = context;

        public async Task AddAsync(Product product) => await _context.Products.AddAsync(product);

        public async Task DeleteAsync(int id)
        {
            var p = await _context.Products.FindAsync(id);
            if (p != null) _context.Products.Remove(p);
        }

        //Customers will see only those products where stock>0
        public async Task<IEnumerable<Product>> GetAllAvailableAsync()
        {
            //select * from Table where stock>0
            return await _context.Products
                .Where(p => p.StockQuantity > 0)
                .AsNoTracking()
                .ToListAsync();
        }

        //Admin will see list of all the products
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            //select * from Table
            return await _context.Products
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id) => await _context.Products.FindAsync(id);

        public Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}