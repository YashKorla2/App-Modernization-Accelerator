using System;
using System.Threading.Tasks;
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

        public async Task<IActionResult> Index(string searchTerm)
        {
            var Carts = string.IsNullOrEmpty(searchTerm)
                ? await _cartService.GetCartsAsync()
                : await _cartService.SearchCartAsync(searchTerm);

            ViewBag.SearchTerm = searchTerm;

            return View(Carts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _cartService.DeleteCartItemAsync(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(int[] selectedItems)
        {
            if (selectedItems == null || selectedItems.Length == 0)
                return RedirectToAction("Index");

            await _cartService.CheckoutAsync(selectedItems);
            return RedirectToAction("Index");
        }
    }
}