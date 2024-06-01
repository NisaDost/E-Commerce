using ECOMMERCE2.Data;
using ECOMMERCE2.Data.Model;
using ECOMMERCE2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

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
    }
}
