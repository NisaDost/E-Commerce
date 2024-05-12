using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
