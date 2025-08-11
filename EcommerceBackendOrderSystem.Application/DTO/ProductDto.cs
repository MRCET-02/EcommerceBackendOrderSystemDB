using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceBackendOrderSystem.Application.DTO
{
    public class ProductDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string? Name { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be >= 0")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "StockQuantity must be >= 0")]
        public int StockQuantity { get; set; }

        [Required]
        [StringLength(100)]
        public string? Category { get; set; }
    }
}

