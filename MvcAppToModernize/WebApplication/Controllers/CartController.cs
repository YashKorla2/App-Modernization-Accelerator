using Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


namespace WebApplication.Controllers
{
    /// <summary>
    /// Controller responsible for handling shopping cart related operations
    /// including viewing, searching, deleting items and checkout
    /// </summary>
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        /// <summary>
        /// Constructor that injects the cart service dependency
        /// The C# file of this dependency can be found in MvcAppToModernize\Services folder.
        /// </summary>
        /// <param name="cartService">Service for cart operations</param>
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        
        /// <summary>
        /// Displays the cart contents and handles search functionality
        /// </summary>
        /// <param name="searchTerm">Optional search term to filter cart items</param>
        /// <returns>View displaying cart items, filtered by search term if provided</returns>
        public IActionResult Index(string searchTerm)
        {
            IEnumerable<object> carts = string.IsNullOrEmpty(searchTerm)
                ? _cartService.GetCarts()
                : _cartService.SearchCart(searchTerm);

            ViewBag.SearchTerm = searchTerm;

            return View(carts);
        }

        /// <summary>
        /// Removes a specific item from the cart
        /// </summary>
        /// <param name="id">ID of the cart item to delete</param>
        /// <returns>Redirects back to cart index page</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            _cartService.DeleteCartItem(id);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Processes checkout for selected cart items
        /// </summary>
        /// <param name="selectedItems">Array of item IDs selected for checkout</param>
        /// <returns>Redirects back to cart index page after checkout</returns>
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