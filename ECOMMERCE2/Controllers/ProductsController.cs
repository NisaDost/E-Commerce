using ECOMMERCE2.Data;
using ECOMMERCE2.Data.Model;
using ECOMMERCE2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ECOMMERCE2.Controllers
{
    public class ProductsController : Controller
    {
        private readonly DBContext _context;
        private readonly int PageSize = 6;

        public ProductsController(DBContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1)
        {
            var categories = _context.Categories.ToList();
            var products = _context.Products.OrderBy(p => p.Price).Skip((page-1)*PageSize).Take(PageSize).ToList();
            ViewBag.Categories = categories;
            return View(products);
        }

        public IActionResult Details(int id)
        {
            var product = _context.Products
                           .Include(p => p.Category) // Include the Category navigation property
                           .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        public IActionResult AddProduct()
        {
            var categories = _context.Categories.ToList();
            var brands = _context.Products.ToList();
            ViewBag.Categories = categories;
            ViewBag.Brands = brands;
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> AddProductAsync(ProductAddViewModel product)
        {
            var fileName = Path.GetFileName(product.Image.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await product.Image.CopyToAsync(fileStream);
            }

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
                Picture = "/img/" + fileName
            };

            _context.Products.Add(newProduct);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult EditProduct(int id)
        {
            var product = _context.Products.Find(id);
            var brands = _context.Products.ToList();
            ViewBag.Brands = brands;
            if (product == null)
            {
                return NotFound();
            }
            var categories = _context.Categories.ToList();
            ViewBag.Categories = categories;
            var productViewModel = new ProductEditViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Brand = product.Brand,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                InStock = product.InStock,
                CategoryId = product.CategoryId
            };

            return View(productViewModel);
        }

        [HttpPost]
        public IActionResult EditProduct(ProductEditViewModel productViewModel)
        {
            var category = _context.Categories.Find(productViewModel.CategoryId);
            var product = _context.Products.Find(productViewModel.Id);
            var brands = _context.Products.ToList();

            product.Name = productViewModel.Name;
            product.Brand = productViewModel.Brand;
            product.Description = productViewModel.Description;
            product.Price = productViewModel.Price;
            product.StockQuantity = productViewModel.StockQuantity;
            product.InStock = productViewModel.InStock;
            product.Category = category;
            product.CategoryId = productViewModel.CategoryId;

            if (productViewModel.Image != null)
            {
                var fileName = Path.GetFileName(productViewModel.Image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    productViewModel.Image.CopyTo(fileStream);
                }
                product.Picture = "/img/" + fileName;
            }

            _context.Products.Update(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
