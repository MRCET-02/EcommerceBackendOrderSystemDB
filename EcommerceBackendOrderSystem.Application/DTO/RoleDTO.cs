using EcommerceBackendOrderSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceBackendOrderSystem.Application.DTO
{
    public class RoleDTO
    {
        public int Id { get; set; }


        [Required]
        [StringLength(30, MinimumLength = 2)]
        public RoleType Name { get; set; }
    }
}
