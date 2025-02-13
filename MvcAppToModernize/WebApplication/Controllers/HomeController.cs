using System.Web.Mvc;
using Services;
using Models;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;

        public HomeController() {}

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        public ActionResult Index()
        {
            var products = _homeService.GetAllProducts();
            return View(products);
        }

        public ActionResult Details(int id)
        {
            var product = _homeService.GetProductById(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _homeService.AddProduct(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public ActionResult Edit(int id)
        {
            var product = _homeService.GetProductById(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _homeService.UpdateProduct(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public ActionResult Delete(int id)
        {
            var product = _homeService.GetProductById(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            _homeService.DeleteProduct(id);
            return RedirectToAction("Index");
        }
    }
}