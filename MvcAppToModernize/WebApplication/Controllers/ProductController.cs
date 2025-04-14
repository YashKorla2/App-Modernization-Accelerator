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
public class ProductViewModel
{
    public IEnumerable<Product> Products { get; set; }
    public int CartItemCount { get; set; }
}

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
        IEnumerable<Product> products = string.IsNullOrEmpty(searchTerm)
            ? _productService.GetAllProducts()
            : _productService.SearchProducts(searchTerm);
        var cartItems = _cartService.GetCarts();

        var viewModel = new ProductViewModel
        {
            Products = products,
            CartItemCount = cartItems?.Sum(c => ((dynamic)c).Quantity) ?? 0
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
            return View(product);
        }

        [HttpGet("create")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public ActionResult<Product> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _productService.AddProduct(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [HttpGet("edit/{id}")]
        public ActionResult<Product> Edit(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost("edit")]
        public ActionResult<Product> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _productService.UpdateProduct(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [HttpGet("delete/{id}")]
        public ActionResult<Product> Delete(int id)
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