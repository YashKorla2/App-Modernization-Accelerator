using System;
using System.Collections.Generic;
using System.Linq;
using Services;
using Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication.Controllers
{
    public class ProductViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public int CartItemCount { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public ProductController() {}

        public ProductController(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }

        [HttpGet]
        public ActionResult<ProductViewModel> Index(string searchTerm)
        {
            System.Collections.Generic.IEnumerable<Product> products = string.IsNullOrEmpty(searchTerm)
                ? _productService.GetAllProducts()
                : _productService.SearchProducts(searchTerm);
            System.Collections.Generic.List<Cart> cartItems = new System.Collections.Generic.List<Cart>(_cartService.GetCarts());

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
            var product = _productService.GetProductById(productId);
            if (product != null)
            {
                _cartService.AddProductToCart(product, quantity);
            }

            return RedirectToAction("Index");
        }
    }
}