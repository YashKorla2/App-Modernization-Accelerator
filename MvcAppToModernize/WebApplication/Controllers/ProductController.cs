using System.Threading.Tasks;
using System.Web.Mvc;
using Services;
using Models;

namespace WebApplication.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public ProductController() {}

        public ProductController(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<ActionResult> Index(string searchTerm)
        {
            var products = string.IsNullOrEmpty(searchTerm)
                ? await _productService.GetAllProductsAsync()
                : await _productService.SearchProductsAsync(searchTerm);

            var cartItems = await _cartService.GetCartsAsync();

            var viewModel = new ProductViewModel
            {
                Products = products,
                CartItemCount = cartItems.Count
            };

            ViewBag.SearchTerm = searchTerm;
            return View(viewModel);
        }

        public async Task<ActionResult> Details(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                await _productService.AddProductAsync(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                await _productService.UpdateProductAsync(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public async Task<ActionResult> Delete(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _productService.DeleteProductAsync(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> AddToCart(int productId, int quantity = 1)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product != null)
            {
                await _cartService.AddProductToCartAsync(product, quantity);
            }

            return RedirectToAction("Index");
        }
    }
}
