/// Hint for Q: Assume that the ICartService interface is available in the MvcAppToModernize\Services directory
/// The cartService object uses the methods of the ICartService interface which are implemented in the CartService class
/// which is also available in the MvcAppToModernize\Services directory.

using System.Collections.Generic;
using System.Web.Mvc;
using Services;

namespace WebApplication.Controllers
{
    // Controller responsible for handling order-related operations and views
    public class OrderController : Controller
    {
        // Service dependency for cart and order operations
        private readonly ICartService _cartService;

        // Constructor injection of cart service dependency
        public OrderController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // GET action that displays orders list with optional search functionality
        // Parameters:
        //   searchTerm - Optional search string to filter orders
        // Returns: View containing filtered or all orders
        public ActionResult Index(string searchTerm)
        {
            // Get either all orders or search results based on search term
            var orders = string.IsNullOrEmpty(searchTerm)
                ? _cartService.GetOrders()
                : _cartService.SearchOrders(searchTerm);
            
            var orderCount = orders.Count;

            // Pass order count and search term to view via ViewBag
            ViewBag.OrderCount = orderCount;
            ViewBag.SearchTerm = searchTerm;

            // Return view with orders as model
            return View(orders);
        }
    }
}
