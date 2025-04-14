using System;
using System.Collections.Generic;
using System.Linq;
using Services;
using Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace WebApplication.Controllers
{
    public class ProductViewModel
    {
        public IEnumerable<Product> Products { get; set; } = new List<Product>();
        public int CartItemCount { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ICartService cartService, ILogger<ProductController> logger)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public ActionResult<ProductViewModel> Index(string searchTerm)
        {
            IEnumerable<Product> products = string.IsNullOrEmpty(searchTerm)
                ? _productService.GetAllProducts()
                : _productService.SearchProducts(searchTerm);
            List<Cart> cartItems = _cartService.GetCarts().ToList();

            var viewModel = new ProductViewModel
            {
                Products = products,
                CartItemCount = cartItems?.Sum(c => c.Quantity) ?? 0
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

        [HttpGet("create")]
        public IActionResult Create()
        {
            return Ok();
        }

        [HttpPost("create")]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _productService.AddProduct(product);
                return RedirectToAction("Index");
            }
            return BadRequest(ModelState);
        }

        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost("edit")]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _productService.UpdateProduct(product);
                return RedirectToAction("Index");
            }
            return BadRequest(ModelState);
        }

        [HttpGet("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost("delete/{id}")]
        public ActionResult DeleteConfirmed(int id)
        {
            _productService.DeleteProduct(id);
            return RedirectToAction("Index");
        }

        [HttpPost("addtocart")]
        public ActionResult AddToCart(int productId, int quantity = 1)
        {
            try
            {
                var product = _productService.GetProductById(productId);
                if (product != null)
                {
                    _cartService.AddProductToCart(product, quantity);
                    _logger.LogInformation($"Added product {productId} to cart, quantity: {quantity}");
                }
                else
                {
                    _logger.LogWarning($"Attempted to add non-existent product {productId} to cart");
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding product {productId} to cart");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}