using System.Web.Mvc;

namespace MvcAppToModernize.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            // Returning a simple view with a message
            ViewBag.Title = "Home Page";
            return View();
        }

        // GET: Home/About
        public ActionResult About()
        {
            return View();
        }

        // GET: Home/Contact
        public ActionResult Contact()
        {
            return View();
        }
    }
}
