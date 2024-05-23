using Microsoft.AspNetCore.Mvc;

namespace ECOMMERCE2.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Checkout()
        {
            return View();
        }
    }
}
