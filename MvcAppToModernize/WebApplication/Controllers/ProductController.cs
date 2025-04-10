using System;
using System.Collections.Generic;
using System.Linq;
using Services;
using Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebApplication.Controllers
{
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

        public IActionResult Index(string searchTerm)
        {
            System.Collections.Generic.IEnumerable<Product> products = string.IsNullOrEmpty(searchTerm)
                ? _productService.GetAllProducts()
                : _productService.SearchProducts(searchTerm);
            System.Collections.Generic.IEnumerable<Cart> cartItems = _cartService.GetCarts();

            var viewModel = new ProductViewModel
            {
                Products = products.ToList(),
                CartItemCount = cartItems.Count()
            };

            ViewBag.SearchTerm = searchTerm;

            return View(viewModel);
        }

        public IActionResult Details(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _productService.AddProduct(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public IActionResult Edit(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _productService.UpdateProduct(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public IActionResult Delete(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _productService.DeleteProduct(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
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