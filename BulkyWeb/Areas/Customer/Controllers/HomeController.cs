using System.Diagnostics;
using Bulky.DataAccess.Services.IServices;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IInternal _internal;

        public HomeController(ILogger<HomeController> logger, IInternal @internal)
        {
            _logger = logger;
            _internal = @internal;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> ProductList = _internal.ProductRepository.GetAll(includeProperties: "Category");
            return View(ProductList);
        }

        public IActionResult Details(int? productId)
        {
            Product Product = _internal.ProductRepository.Get(u => u.Id == productId, includeProperties:"Category");
            return View(Product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
