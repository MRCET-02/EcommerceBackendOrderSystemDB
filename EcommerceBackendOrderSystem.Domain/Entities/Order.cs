using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcommerceBackendOrderSystem.Domain.Enums;

namespace EcommerceBackendOrderSystem.Domain.Entities
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public virtual User Customer { get; set; }
        public int? DeliveryAgentId { get; set; }
        public virtual User DeliveryAgent{ get; set; }
        public DateTime OrderDate { get; set; }

        public OrderStatus Status{ get; set; } = OrderStatus.Pending;
        public PaymentStatus PaymentStatus{ get; set; }= PaymentStatus.Pending;
        public virtual ICollection<OrderItem>OrderItems { get; set; }
      
       
    }
}
