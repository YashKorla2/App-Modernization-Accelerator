using Services;
using Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

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
        public ActionResult<ProductViewModel> Index(string? searchTerm)
        {
            IEnumerable<Product> products = string.IsNullOrEmpty(searchTerm)
                ? _productService.GetAllProducts()
                : _productService.SearchProducts(searchTerm);
            var cartItems = _cartService.GetCarts();

            var viewModel = new ProductViewModel
            {
                Products = products.ToList(),
                CartItemCount = cartItems.Count()
            };

            return viewModel;
        }

        [HttpGet("{id}")]
        public ActionResult<Product> Details(System.Int32 id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public ActionResult<Product> Create([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _productService.AddProduct(product);
            return CreatedAtAction(nameof(Details), new { id = product }, product);
        }

        [HttpPut("{id}")]
        public IActionResult Edit(System.Int32 id, [FromBody] Product product)
        {
            var existingProduct = _productService.GetProductById(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _productService.UpdateProduct(product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(System.Int32 id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            _productService.DeleteProduct(id);
            return NoContent();
        }

        [HttpPost("AddToCart")]
        public IActionResult AddToCart(System.Int32 productId, System.Int32 quantity = 1)
        {
            var product = _productService.GetProductById(productId);
            if (product == null)
            {
                return NotFound();
            }

            _cartService.AddProductToCart(product, quantity);
            return Ok();
        }
    }
}