using ECOMMERCE2.Data;
using Microsoft.AspNetCore.Mvc;

namespace ECOMMERCE2.Controllers
{
    public class AdminController : Controller
    {
        private readonly DBContext _context;
        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            ViewBag.Products = products;
            return View();
        }
        public IActionResult AddCategory()
        {
            return View();
        }
        public IActionResult AddBrand()
        {
            return View();
        }
    }
}
