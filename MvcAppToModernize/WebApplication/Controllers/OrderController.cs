using Microsoft.AspNetCore.Mvc;
using Services;

namespace WebApplication.Controllers
{
    public class OrderController : Controller
    {
        private readonly ICartService _cartService;

        public OrderController(ICartService cartService)
        {
            _cartService = cartService;
        }

        public IActionResult Index(string searchTerm)
        {
            IEnumerable<object> ordersEnumerable = string.IsNullOrEmpty(searchTerm)
                ? _cartService.GetOrders()
                : _cartService.SearchOrders(searchTerm);

            List<object> orders = ordersEnumerable.ToList();

            ViewBag.OrderCount = orders.Count;
            ViewBag.SearchTerm = searchTerm;

            return View(orders);
        }
    }
}