using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceBackendOrderSystem.Application.DTO;
using EcommerceBackendOrderSystem.Application.Services;
using EcommerceBackendOrderSystem.Domain.Entities;
using EcommerceBackendOrderSystem.Domain.Enums;
using EcommerceBackendOrderSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace EcommerceBackendOrderSystem.UnitTestCases.UnitTests
{
    [TestFixture]
    public class OrderFlowTests
    {
        private ApplicationDbContext _context;
        private OrderService _orderService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            // Seed data as before
            _context.Products.AddRange(
                new Product { Id = 1, Name = "Product A", StockQuantity = 10, Price = 100, Category = "Category1" },
                new Product { Id = 2, Name = "Product B", StockQuantity = 5, Price = 50, Category = "Category1" }
            );

            _context.Users.Add(new User
            {
                Id = 1,
                Username = "customer1",
                Email = "customer1@gmail.com",
                PasswordHash = "hashedpassword"
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
            var orderRequest = new OrderRequest
            {
                CustomerId = 1,
                OrderItems = new List<OrderItemRequest>
                {
                    new OrderItemRequest { ProductId = 1, Quantity = 3 },
                    new OrderItemRequest { ProductId = 2, Quantity = 2 }
                }
            };

            var response = await _orderService.CreateOrderAsync(orderRequest);

            Assert.NotNull(response);
            Assert.AreEqual(1, response.CustomerId);
            Assert.AreEqual(2, response.OrderItems.Count);

            var product1 = await _context.Products.FindAsync(1);
            var product2 = await _context.Products.FindAsync(2);

            Assert.AreEqual(7, product1.StockQuantity); // 10 - 3
            Assert.AreEqual(3, product2.StockQuantity); // 5 - 2
        }

        [Test]
        public void CreateOrderAsync_Should_ThrowException_WhenStockInsufficient()
        {
            var orderRequest = new OrderRequest
            {
                CustomerId = 1,
                OrderItems = new List<OrderItemRequest>
                {
                    new OrderItemRequest { ProductId = 1, Quantity = 20 } // exceed stock
                }
            };

            var ex = Assert.ThrowsAsync<Exception>(async () => await _orderService.CreateOrderAsync(orderRequest));
            Assert.That(ex.Message, Does.Contain("Not enough stock"));
        }

        [Test]
        public async Task UpdateOrderStatus_Should_ChangeStatus()
        {
            var orderRequest = new OrderRequest
            {
                CustomerId = 1,
                OrderItems = new List<OrderItemRequest>
                {
                    new OrderItemRequest { ProductId = 1, Quantity = 1 }
                }
            };

            var response = await _orderService.CreateOrderAsync(orderRequest);

            var result = await _orderService.UpdateOrderStatusAsync(response.OrderId, OrderStatus.Shipped);

            Assert.IsTrue(result);

            var updatedOrder = await _context.Orders.FindAsync(response.OrderId);
            Assert.AreEqual(OrderStatus.Shipped, updatedOrder.Status);
        }

        [Test]
        public async Task GetAllOrdersAsync_Should_ReturnAllOrders()
        {
            var orderRequest = new OrderRequest
            {
                CustomerId = 1,
                OrderItems = new List<OrderItemRequest>
                {
                    new OrderItemRequest { ProductId = 1, Quantity = 1 }
                }
            };

            await _orderService.CreateOrderAsync(orderRequest);

            var allOrders = await _orderService.GetAllOrdersAsync();

            Assert.IsNotEmpty(allOrders);
            Assert.AreEqual(1, allOrders.Count);
        }
    }
}
