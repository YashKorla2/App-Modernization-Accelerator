using System.Collections.Generic;
using Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using YourNamespace.Models; // Add this line, replace YourNamespace.Models with the actual namespace of the Order class


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
        IEnumerable<Order> orders = string.IsNullOrEmpty(searchTerm)
            ? _cartService.GetOrders()
            : _cartService.SearchOrders(searchTerm);

        var orderCount = orders.Count();

        ViewBag.OrderCount = orderCount;
        ViewBag.SearchTerm = searchTerm;

        return View(orders);
    }
    }
}