using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Services;

namespace WebApplication.Controllers
{
    /// <summary>
    /// Controller responsible for handling all product-related operations including
    /// viewing, creating, editing, deleting products and managing shopping cart
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        /// <summary>
        /// Constructor that initializes product and cart services through dependency injection
        /// </summary>
        public ProductController(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }

        /// <summary>
        /// Displays list of all products with optional search functionality
        /// Returns a view with products and cart item count
        /// </summary>
        [HttpGet]
        public ActionResult<object> Index(string? searchTerm)
        {
            IEnumerable<Product> products = string.IsNullOrEmpty(searchTerm)
                ? _productService.GetAllProducts()
                : _productService.SearchProducts(searchTerm);
            IEnumerable<Cart> cartItems = _cartService.GetCarts();

            var viewModel = new
            {
                Products = products.ToList(),
                CartItemCount = cartItems.Count()
            };

            return Ok(viewModel);
        }

        /// <summary>
        /// Displays detailed information for a specific product
        /// Returns 404 if product is not found
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<object> Details(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        /// <summary>
        /// Displays form for creating a new product
        /// </summary>
        [HttpGet("Create")]
        public ActionResult<object> Create()
        {
            return Ok(new { message = "Create form loaded" });
        }

        /// <summary>
        /// Handles the POST request to create a new product
        /// Validates the model and redirects to Index on success
        /// </summary>
        [HttpPost("Create")]
        public ActionResult<object> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _productService.AddProduct(product);
                return CreatedAtAction(nameof(Details), new { id = product.Id }, product);
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Displays form for editing an existing product
        /// Returns 404 if product is not found
        /// </summary>
        public IActionResult Edit(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        /// <summary>
        /// Handles the POST request to update an existing product
        /// Validates the model and redirects to Index on success
        /// </summary>
        [HttpPost("Edit")]
        public ActionResult<object> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _productService.UpdateProduct(product);
                return Ok(new { message = "Product updated successfully" });
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Displays confirmation page for deleting a product
        /// Returns 404 if product is not found
        /// </summary>
        public IActionResult Delete(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpGet]
        public IActionResult DeleteConfirmation(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        /// <summary>
        /// Handles the POST request to delete a product
        /// Redirects to Index after deletion
        /// </summary>
        [HttpPost("Delete/{id}")]
        public IActionResult DeleteConfirmed(int id)
        {
            _productService.DeleteProduct(id);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Handles adding products to the shopping cart
        /// Accepts product ID and optional quantity (defaults to 1)
        /// </summary>
        [HttpPost("AddToCart")]
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            var product = _productService.GetProductById(productId);
            if (product != null)
            {
                _cartService.AddProductToCart(product, quantity);
            }

            return RedirectToAction("Index");
        }
    }
}