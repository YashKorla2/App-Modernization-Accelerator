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

        [HttpGet]
        public ActionResult<Models.ProductViewModel> Index(string searchTerm)
        {
            System.Collections.Generic.IEnumerable<Models.Product> products = string.IsNullOrEmpty(searchTerm)
                ? _productService.GetAllProducts()
                : _productService.SearchProducts(searchTerm);
            System.Collections.Generic.IEnumerable<Models.Cart> cartItems = _cartService.GetCarts();

            var viewModel = new Models.ProductViewModel
            {
                Products = products,
                CartItemCount = cartItems.Count()
            };

            // Remove ViewBag usage as it's not available in ControllerBase
            // Instead, we'll pass the searchTerm in the view model

            return View(viewModel);
        }

        [HttpGet("{id}")]
        public ActionResult<Models.Product> Details(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpGet("create")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public ActionResult<Models.Product> Create(Models.Product product)
        {
            if (ModelState.IsValid)
            {
                _productService.AddProduct(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [HttpGet("edit/{id}")]
        public ActionResult<Models.Product> Edit(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost("edit")]
        public ActionResult<Models.Product> Edit(Models.Product product)
        {
            if (ModelState.IsValid)
            {
                _productService.UpdateProduct(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [HttpGet("delete/{id}")]
        public ActionResult<Models.Product> Delete(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
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