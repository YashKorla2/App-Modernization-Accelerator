using System.Threading.Tasks;
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

        public async Task<ActionResult> Index(string searchTerm)
        {
            var orders = string.IsNullOrEmpty(searchTerm)
                ? await _cartService.GetOrdersAsync()
                : await _cartService.SearchOrdersAsync(searchTerm);

            ViewBag.OrderCount = orders.Count;
            ViewBag.SearchTerm = searchTerm;

            return View(orders);
        }
    }
}
