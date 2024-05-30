using ECOMMERCE2.Data.Model;
using Microsoft.AspNetCore.Mvc;

namespace ECOMMERCE2.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            var role = new User().Role;
            ViewBag.Role = role;
            return RedirectToAction("Index", "Home", role);
        }
    }
}
