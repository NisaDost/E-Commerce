using Microsoft.AspNetCore.Mvc;

namespace ECOMMERCE2.Controllers
{
    public class OrdersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
