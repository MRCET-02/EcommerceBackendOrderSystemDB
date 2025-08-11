using System;
using EcommerceBackendOrderSystem.Domain.Enums;

namespace EcommerceBackendOrderSystem.Application.DTO
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public OrderStatus DeliveryStatus { get; set; }
    }
}