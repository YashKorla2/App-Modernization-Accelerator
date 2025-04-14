using System;
using System.Collections.Generic;
using System.Linq;
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

        public ActionResult Index(string searchTerm)
        {
            IEnumerable<object> ordersEnumerable = string.IsNullOrEmpty(searchTerm)
                ? _cartService.GetOrders()
                : _cartService.SearchOrders(searchTerm);

            var orders = ordersEnumerable.ToList<object>();

            var orderCount = orders.Count;

            ViewBag.OrderCount = orderCount;
            ViewBag.SearchTerm = searchTerm;

            return View(orders);
        }
    }
}