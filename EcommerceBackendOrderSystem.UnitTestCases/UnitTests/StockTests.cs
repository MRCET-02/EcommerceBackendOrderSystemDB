using NUnit.Framework;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using EcommerceBackendOrderSystem.Application.DTO;
using EcommerceBackendOrderSystem.Application.Services;
using EcommerceBackendOrderSystem.Domain.Entities;
using EcommerceBackendOrderSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EcommerceBackendOrderSystem.UnitTests
{
    [TestFixture]
    public class OrderServiceTests
    {
        private ApplicationDbContext _context;
        private OrderService _orderService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;

            _context = new ApplicationDbContext(options);

           
            _context.Products.Add(new Product
            {
                Id = 1,
                Name = "Test Product",
                Category = "TestCategory", 
                Price = 100,
                StockQuantity = 10
            });

        
            _context.Users.Add(new User
            {
                Id = 1,
                Username = "testuser",
                Email = "testuser@example.com",        
                PasswordHash = "hashedpassword",       
                                                        
            });

            _context.SaveChanges();

            _orderService = new OrderService(_context);
        }


        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task CreateOrderAsync_Should_DecrementStock_WhenOrderIsValid()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                CustomerId = 1,
                OrderItems = new List<OrderItemRequest>
                {
                    new OrderItemRequest { ProductId = 1, Quantity = 3 }
                }
            };

            // Act
            var response = await _orderService.CreateOrderAsync(orderRequest);

            // Assert
            var product = await _context.Products.FindAsync(1);
            Assert.AreEqual(7, product.StockQuantity);
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.CustomerId);
            Assert.AreEqual(1, response.OrderItems.Count);
            Assert.AreEqual(3, response.OrderItems[0].Quantity);
        }

        [Test]
        public void CreateOrderAsync_ShouldThrowException_WhenProductStockIsInsufficient()
        {
            // Arrange
            var orderRequest = new OrderRequest
            {
                CustomerId = 1,
                OrderItems = new List<OrderItemRequest>
                {
                    new OrderItemRequest { ProductId = 1, Quantity = 100 } // exceeds stock
                }
            };

            // Act & Assert
            Assert.ThrowsAsync<System.Exception>(async () =>
                await _orderService.CreateOrderAsync(orderRequest));
        }
    }
}
