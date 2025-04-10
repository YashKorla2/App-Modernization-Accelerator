using System.Collections.Generic;
using System.Linq;
using Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    public class OrderController : Controller
    {
        private readonly ICartService _cartService;

        public OrderController(ICartService cartService)
        {
            _cartService = cartService;
        }

        public ActionResult Index(string searchTerm)
        {
            List<object> orders = string.IsNullOrEmpty(searchTerm)
                ? _cartService.GetOrders().ToList<object>()
                : _cartService.SearchOrders(searchTerm).ToList<object>();

            int orderCount = orders.Count;

            ViewBag.OrderCount = orderCount;
            ViewBag.SearchTerm = searchTerm;

            return View(orders);
        }
    }
}