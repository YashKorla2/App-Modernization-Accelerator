using Services;
using Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Controllers
{
    public class ProductViewModel
    {
        public List<Product> Products { get; set; }
        public int CartItemCount { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public ProductController(IProductService productService, ICartService cartService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
        }

        [HttpGet]
        public async Task<ActionResult<ProductViewModel>> Index(string? searchTerm)
        {
            IEnumerable<Product> products = string.IsNullOrEmpty(searchTerm)
                ? await _productService.GetAllProductsAsync()
                : await _productService.SearchProductsAsync(searchTerm);
            var cartItems = await _cartService.GetCartsAsync();

            var viewModel = new ProductViewModel
            {
                Products = products.ToList(),
                CartItemCount = cartItems.Count()
            };

            return viewModel;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Details(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Create([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _productService.AddProductAsync(product);
            return CreatedAtAction(nameof(Details), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Product product)
        {
            var existingProduct = await _productService.GetProductByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _productService.UpdateProductAsync(product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await _productService.DeleteProductAsync(id);
            return NoContent();
        }

        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            await _cartService.AddProductToCartAsync(product, quantity);
            return Ok();
        }
    }
}