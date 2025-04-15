using System;
using System.Collections.Generic;
using System.Linq;
using Services;
using Models;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult<ProductViewModel> Index(string searchTerm)
        {
            var products = string.IsNullOrEmpty(searchTerm)
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
        public ActionResult<Product> Details(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        [HttpPost]
        public ActionResult<Product> Create([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _productService.AddProduct(product);
            return CreatedAtAction(nameof(Details), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public IActionResult Edit(int id, [FromBody] Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

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
        public IActionResult Delete(int id)
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
        public IActionResult AddToCart(int productId, int quantity = 1)
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