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
            var newCategoty = new Category
            {
                Name = name,
                Icon = icon
            };
            _context.Categories.Add(newCategoty);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult EditCategory(int id)
        {
            return View();
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
