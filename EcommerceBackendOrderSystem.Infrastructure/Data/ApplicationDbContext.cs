using EcommerceBackendOrderSystem.Domain.Entities;
using EcommerceBackendOrderSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace EcommerceBackendOrderSystem.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<UserRole>()
            .HasIndex(ur => new { ur.UserId, ur.RoleId })
            .IsUnique();


            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Order>()
                .HasOne(o => o.DeliveryAgent)
                .WithMany()
                .HasForeignKey(o => o.DeliveryAgentId)
                .OnDelete(DeleteBehavior.SetNull);


            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(12, 2); 

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Price)
                .HasPrecision(12, 2);



            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Name)
                .IsUnique();


            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<string>();
            modelBuilder.Entity<Role>()
    .Property(r => r.Name)
    .HasConversion<string>();


        }
    }
}
