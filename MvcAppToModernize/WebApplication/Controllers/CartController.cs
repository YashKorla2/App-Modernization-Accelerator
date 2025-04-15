using Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


namespace WebApplication.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        
        public ActionResult Index(string searchTerm)
        {
            IEnumerable<Cart> carts = string.IsNullOrEmpty(searchTerm)
                ? _cartService.GetCarts()
                : _cartService.SearchCart(searchTerm);

            ViewBag.SearchTerm = searchTerm;

            return View(carts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            _cartService.DeleteCartItem(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Checkout(int[] selectedItems)
        {
            if (selectedItems == null || selectedItems.Length == 0)
                return RedirectToAction("Index");

            _cartService.Checkout(selectedItems);
            return RedirectToAction("Index");
        }
    }
}