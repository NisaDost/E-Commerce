using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECOMMERCE2.Data;
using ECOMMERCE2.Data.Model;
using ECOMMERCE2.Models;

namespace ECOMMERCE2.Controllers
{
    public class AdminController : Controller
    {
        private readonly DBContext _context;

        public AdminController(DBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(string name, string icon)
        {
            var newCategory = new Category
            {
                Name = name,
                Icon = icon
            };
            _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(int id, string name, string icon)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            category.Name = name;
            category.Icon = icon;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return Json(new { success = false, message = "Category not found" });
            }

            var productsCount = await _context.Products.CountAsync(p => p.CategoryId == id);
            if (productsCount > 0)
            {
                return Json(new { success = false, message = "There are products in this category!" });
            }

            category.IsDeleted = true;
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        public IActionResult EditUser(string search = null)
        {
            var users = string.IsNullOrEmpty(search)
                ? _context.Users.Where(u => !u.IsDeleted).ToList()
                : _context.Users.Where(u => !u.IsDeleted && u.Username.Contains(search)).ToList();
            var userViewModels = users.Select(u => new UserViewModel
            {
                UserId = u.UserId,
                Username = u.Username,
                Name = u.Name,
                Surname = u.Surname,
                Role = u.Role
            }).ToList();
            return View(userViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> SaveUserRoles([FromBody] List<UserRoleUpdateModel> updates)
        {
            foreach (var update in updates)
            {
                var user = await _context.Users.FindAsync(update.UserId);
                if (user != null && user.Role != "Admin") // Only update if the user is not an admin
                {
                    user.Role = update.Role;
                }
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null) 
            {
                user.IsDeleted = true;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("EditUser");
        }
    }

    public class UserRoleUpdateModel
    {
        public int UserId { get; set; }
        public string Role { get; set; }
    }
}
