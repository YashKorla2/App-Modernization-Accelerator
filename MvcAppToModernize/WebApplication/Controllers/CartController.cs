using Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        public IActionResult Index(string searchTerm)
        {
            var carts = string.IsNullOrEmpty(searchTerm)
                ? _cartService.GetCarts()
                : _cartService.SearchCart(searchTerm);

            ViewData["SearchTerm"] = searchTerm;

            return View(carts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _cartService.DeleteCartItem(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Checkout(int[] selectedItems)
        {
            if (selectedItems == null || selectedItems.Length == 0)
                return RedirectToAction(nameof(Index));

            _cartService.Checkout(selectedItems);
            return RedirectToAction(nameof(Index));
        }
    }
}