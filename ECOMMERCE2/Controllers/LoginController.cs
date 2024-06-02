using ECOMMERCE2.Data.Model;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ECOMMERCE2.Models;
using ECOMMERCE2.Data;
using Microsoft.EntityFrameworkCore;

namespace ECOMMERCE2.Controllers
{
    public class LoginController : Controller
    {
        private readonly DBContext _context;
        public LoginController(DBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginViewModel loginViewModel)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginViewModel.Username && u.Password == loginViewModel.Password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new(ClaimTypes.Name, user.Username),
                    new(ClaimTypes.Role, user.Role.ToString()),
                    new(ClaimTypes.NameIdentifier, user.UserId.ToString())
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { };
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                if (loginViewModel.RememberMe)
                {
                    authProperties.IsPersistent = true;
                    authProperties.ExpiresUtc = DateTime.UtcNow.AddMinutes(10);
                }
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid username or password!";

            return View("Index");
        }
        [HttpGet]
        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel register)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Username = register.Username,
                    Password = register.Password,
                    Name = register.Name,
                    Surname = register.Surname,
                    Role = "User"
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Error = "Invalid data!";
            return View("Register");
        }
    }
}
