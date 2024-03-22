using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
