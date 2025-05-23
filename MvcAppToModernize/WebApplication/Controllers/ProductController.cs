using System.Web.Mvc;
using Services;
using Models;
/// MvcAppToModernize\Services directory.
/// The cartService object uses the methods of the ICartService interface which are implemented in the 
/// CartService class which is also available in the MvcAppToModernize\Services directory.
/// The productService object uses the methods of the IProductService interface which are implemented in the 
/// ProductService class which is also available in the MvcAppToModernize\Services directory.

using System.Web.Mvc;
using Services;
using Models;

namespace WebApplication.Controllers
{
    /// <summary>
    /// Controller responsible for handling all product-related operations including
    /// viewing, creating, editing, deleting products and managing shopping cart
    /// </summary>
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        // Default parameterless constructor
        public ProductController() {}

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
        public ActionResult Index(string searchTerm)
        {
            var products = string.IsNullOrEmpty(searchTerm)
                ? _productService.GetAllProducts()
                : _productService.SearchProducts(searchTerm);
            var cartItems = _cartService.GetCarts();

            var viewModel = new ProductViewModel
            {
                Products = products,
                CartItemCount = cartItems.Count
            };

            ViewBag.SearchTerm = searchTerm;

            return View(viewModel);
        }

        /// <summary>
        /// Displays detailed information for a specific product
        /// Returns 404 if product is not found
        /// </summary>
        public ActionResult Details(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        /// <summary>
        /// Displays form for creating a new product
        /// </summary>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Handles the POST request to create a new product
        /// Validates the model and redirects to Index on success
        /// </summary>
        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _productService.AddProduct(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        /// <summary>
        /// Displays form for editing an existing product
        /// Returns 404 if product is not found
        /// </summary>
        public ActionResult Edit(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        /// <summary>
        /// Handles the POST request to update an existing product
        /// Validates the model and redirects to Index on success
        /// </summary>
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _productService.UpdateProduct(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        /// <summary>
        /// Displays confirmation page for deleting a product
        /// Returns 404 if product is not found
        /// </summary>
        public ActionResult Delete(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        /// <summary>
        /// Handles the POST request to delete a product
        /// Redirects to Index after deletion
        /// </summary>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            _productService.DeleteProduct(id);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Handles adding products to the shopping cart
        /// Accepts product ID and optional quantity (defaults to 1)
        /// </summary>
        [HttpPost]
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
