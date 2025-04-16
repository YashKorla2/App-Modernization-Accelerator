using System;
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

        public IActionResult Index(string searchTerm)
        {
            IEnumerable<object> orders = string.IsNullOrEmpty(searchTerm)
                ? _cartService.GetOrders()
                : _cartService.SearchOrders(searchTerm);

            int orderCount = orders.Count();

            ViewBag.OrderCount = orderCount;
            ViewBag.SearchTerm = searchTerm;

            return View(orders);
        }
    }
}