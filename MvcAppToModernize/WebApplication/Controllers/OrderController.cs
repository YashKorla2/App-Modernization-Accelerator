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
            System.Collections.Generic.IEnumerable<object> orders = string.IsNullOrEmpty(searchTerm)
                ? _cartService.GetOrders()
                : _cartService.SearchOrders(searchTerm);

            var orderCount = orders.Count();

            ViewBag.OrderCount = orderCount;
            ViewBag.SearchTerm = searchTerm;

            return View(orders);
        }
    }
}