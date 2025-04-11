using System.Threading.Tasks;
using System.Web.Mvc;
using Services;

namespace WebApplication.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<ActionResult> Index(string searchTerm)
        {
            var carts = string.IsNullOrEmpty(searchTerm)
                ? await _cartService.GetCartsAsync()
                : await _cartService.SearchCartAsync(searchTerm);

            ViewBag.SearchTerm = searchTerm;
            return View(carts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            await _cartService.DeleteCartItemAsync(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Checkout(int[] selectedItems)
        {
            if (selectedItems == null || selectedItems.Length == 0)
                return RedirectToAction("Index");

            await _cartService.CheckoutAsync(selectedItems);
            return RedirectToAction("Index");
        }
    }
}
