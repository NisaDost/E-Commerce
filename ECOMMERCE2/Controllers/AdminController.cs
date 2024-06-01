using ECOMMERCE2.Data;
using ECOMMERCE2.Data.Model;
using ECOMMERCE2.Models;
using Microsoft.AspNetCore.Mvc;

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

        public IActionResult DeleteCategory(int id)
        {
            var category = _context.Categories.Find(id);
            var products = _context.Products.Where(p => p.CategoryId == id).ToList();
            if (category == null)
            {
                return NotFound();
            }
            if (products.Count <= 0)
            {
                category.IsDeleted = true;
                _context.Categories.Update(category);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
