using Microsoft.AspNetCore.Mvc;
using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication.Controllers
{
    public class ProductViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public int CartItemCount { get; set; }
        public string SearchTerm { get; set; }
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
            IEnumerable<Product> products = string.IsNullOrEmpty(searchTerm)
                ? _productService.GetAllProducts()
                : _productService.SearchProducts(searchTerm);
            var cartItems = _cartService.GetCarts();

            var viewModel = new ProductViewModel
            {
                Products = products,
                CartItemCount = cartItems is ICollection<Cart> collection ? collection.Count : cartItems.Count(),
                SearchTerm = searchTerm
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

        [HttpGet("Create")]
        public ActionResult<Product> Create()
        {
            return new Product();
        }

        [HttpPost("Create")]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _productService.AddProduct(product);
                return CreatedAtAction(nameof(Details), new { id = product.Id }, product);
            }
            return BadRequest(ModelState);
        }

        [HttpGet("Edit/{id}")]
        public ActionResult<Product> Edit(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        [HttpPut("Edit/{id}")]
        public IActionResult Edit(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _productService.UpdateProduct(product);
                return NoContent();
            }
            return BadRequest(ModelState);
        }

        [HttpGet("Delete/{id}")]
        public ActionResult<Product> Delete(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult DeleteConfirmed(int id)
        {
            _productService.DeleteProduct(id);
            return NoContent();
        }

        [HttpPost("AddToCart")]
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            var product = _productService.GetProductById(productId);
            if (product != null)
            {
                _cartService.AddProductToCart(product, quantity);
                return Ok();
            }

            return NotFound();
        }
    }
}