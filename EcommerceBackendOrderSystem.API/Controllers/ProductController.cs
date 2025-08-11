using System.Threading.Tasks;
using EcommerceBackendOrderSystem.Application.DTO;
using EcommerceBackendOrderSystem.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceBackendOrderSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService) => _productService = productService;

        // For All Customers only available products (stock > 0) visible
        [HttpGet]
        [AllowAnonymous] 
        // or [Authorize(Roles = "Customer")] (for only authenticated customers)
        public async Task<IActionResult> GetAvailable()
        {
            var products = await _productService.GetAvailableAsync();
            return Ok(products);
        }

        // For Admin: get all products (including out-of-stock)
        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAdmin()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        // Admin: can create product
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] ProductDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _productService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // Public: get single product by id (only if available)
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null) return NotFound();

            // public see product only if in stock
            if (product.StockQuantity <= 0 && !User.IsInRole("Admin"))
                return NotFound(); // hide out-of-stock product from customers

            return Ok(product);
        }

        // Admin: can update product
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _productService.UpdateAsync(id, dto);
            return NoContent();
        }

        // Admin: can delete product
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);
            return NoContent();
        }
    }
}
