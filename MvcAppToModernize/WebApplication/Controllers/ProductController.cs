using Services;
using Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public ProductController(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }

        [HttpGet]
        public ActionResult<object> Index(string searchTerm)
        {
            IEnumerable<Product> products = string.IsNullOrEmpty(searchTerm)
                ? _productService.GetAllProducts()
                : _productService.SearchProducts(searchTerm);
            var cartItems = _cartService.GetCarts();

            var viewModel = new
            {
                Products = products,
                CartItemCount = cartItems.Count()
            };

            return Ok(viewModel);
        }

        [HttpGet("{id}")]
        public ActionResult<Product> Details(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return Ok();
        }

        [HttpPost("Create")]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _productService.AddProduct(product);
                return RedirectToAction(nameof(Index));
            }
            return BadRequest(ModelState);
        }

        [HttpGet("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost("Edit")]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _productService.UpdateProduct(product);
                return RedirectToAction(nameof(Index));
            }
            return BadRequest(ModelState);
        }

        [HttpGet("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost("Delete/{id}")]
        public IActionResult DeleteConfirmed(int id)
        {
            _productService.DeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("AddToCart")]
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            var product = _productService.GetProductById(productId);
            if (product != null)
            {
                _cartService.AddProductToCart(product, quantity);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}