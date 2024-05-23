using Microsoft.AspNetCore.Mvc;

namespace ECOMMERCE2.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
