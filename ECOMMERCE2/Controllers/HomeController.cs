using ECOMMERCE2.Data;
using ECOMMERCE2.Data.Model;
using ECOMMERCE2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ECOMMERCE2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DBContext _context;

        public HomeController(ILogger<HomeController> logger, DBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            var products = _context.Products.ToList();
            ViewBag.Categories = categories;
            return View(products);
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
