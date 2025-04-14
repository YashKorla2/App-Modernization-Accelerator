using System;
using System.Collections.Generic;
using Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<object>> Index(string searchTerm)
        {
            var carts = string.IsNullOrEmpty(searchTerm)
                ? _cartService.GetCarts()
                : _cartService.SearchCart(searchTerm);

            ViewData["SearchTerm"] = searchTerm;

            return View(carts);
        }

        [HttpPost("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _cartService.DeleteCartItem(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("Checkout")]
        public IActionResult Checkout(int[] selectedItems)
        {
            if (selectedItems == null || selectedItems.Length == 0)
                return RedirectToAction(nameof(Index));

            _cartService.Checkout(selectedItems);
            return RedirectToAction(nameof(Index));
        }
    }
}