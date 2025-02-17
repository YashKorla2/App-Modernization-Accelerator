using System.Collections.Generic;
using System.Web.Mvc;
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

        public ActionResult Index()
        {
            var orders = _cartService.GetOrders(); // Get all orders from the service
            var orderCount = orders.Count; // Get total number of orders
            ViewBag.OrderCount = orderCount;

            return View(orders);
        }
    }
}
