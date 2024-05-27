using ECOMMERCE2.Data;
using ECOMMERCE2.Data.Model;
using ECOMMERCE2.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECOMMERCE2.Controllers
{
    public class ProductsController : Controller
    {
        private readonly DBContext _context;

        public ProductsController(DBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        public IActionResult Details(ProductAddViewModel product)
        {
            return View(product);
        }
        public IActionResult AddProduct()
        {
            var categories = _context.Categories.ToList();
            ViewBag.Categories = categories;
            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(ProductAddViewModel product)
        {
            var category = _context.Categories.Find(product.CategoryId);
            var newProduct = new Product
            {
                Name = product.Name,
                Brand = product.Brand,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                InStock = product.InStock,
                Category = category,
                CategoryId = product.CategoryId,
                Picture = product.Image
            };

            _context.Products.Add(newProduct);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
