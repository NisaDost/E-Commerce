using ECOMMERCE2.Data;
using ECOMMERCE2.Data.Model;
using ECOMMERCE2.Helper;
using ECOMMERCE2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;

namespace ECOMMERCE2.Controllers
{
    public class ProductsController : Controller
    {
        private readonly DBContext _context;
        private readonly int PageSize = 9;

        public ProductsController(DBContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1, List<int> selectedCategories = null, List<string> selectedBrands = null, string orderBy = "asc")
        {
            var categories = _context.Categories.ToList();
            var brands = _context.Products.Select(p => p.Brand).Distinct().ToList();
            int totalProducts = _context.Products.Count();
            ViewBag.Brands = brands;
            ViewBag.CurrentPage = page;
            ViewBag.Categories = categories;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / PageSize);

            var products = new List<Product>();
            IQueryable<Product> productsQueryable = _context.Products;
            if (!User.IsInRole("Admin"))
            {
                productsQueryable = productsQueryable.Where(p => !p.IsDeleted && p.InStock && p.StockQuantity > 0);
            }

            if (selectedCategories != null && selectedCategories.Count > 0)
            {
                productsQueryable = productsQueryable.Where(p => selectedCategories.Contains(p.CategoryId));
            }

            if (selectedBrands != null && selectedBrands.Count > 0)
            {
                productsQueryable = productsQueryable.Where(p => selectedBrands.Contains(p.Brand));
            }

            if (orderBy == "asc")
            {
                productsQueryable = productsQueryable.OrderBy(p => p.Price);
            }
            else if (orderBy == "desc")
            {
                productsQueryable = productsQueryable.OrderByDescending(p => p.Price);
            }

            products = productsQueryable.Skip((page - 1) * PageSize).Take(PageSize).ToList();

            return View(products);
        }

        public IActionResult SearchProducts(string search = null)
        {
            var products = _context.Products.ToList();
            var searchedProducts = _context.Products.Where(p => !p.IsDeleted && p.Name.Contains(search)).ToList();

            if (string.IsNullOrEmpty(search))
            {
                return View(products);
            }

            // Return JSON if the request is AJAX
            if (Request.Headers.XRequestedWith == "XMLHttpRequest" && searchedProducts != null)
            {
                var searchedProductList = searchedProducts.Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Brand = p.Brand,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    InStock = p.InStock,
                    CategoryId = p.CategoryId,
                    Image = p.Picture
                }).ToList();
                return Json(searchedProductList);
            }
            return View(products);
        }

        public IActionResult Details(int id)
        {
            var categories = _context.Categories.ToList();

            // Fetch the selected product with its category
            var product = _context.Products
                           .Include(p => p.Category)
                           .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            // Fetch other products that belong to the same category but exclude the selected product
            var otherProducts = _context.Products
                                .Where(p => p.Id != id && p.CategoryId == product.CategoryId)
                                .ToList();

            ViewBag.Categories = categories;
            ViewBag.OtherProducts = otherProducts;

            return View(product);
        }

        public IActionResult AddProduct()
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            var categories = _context.Categories.ToList();
            var brands = _context.Products.ToList();
            ViewBag.Categories = categories;
            ViewBag.Brands = brands;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProductAsync(ProductAddViewModel product)
        {
            if (product.CategoryId == 0)
            {
                ModelState.AddModelError("CategoryId", "Please select a valid category.");
            }

            if (!ModelState.IsValid)
            {
                var categories = _context.Categories.ToList();
                var brands = _context.Products.ToList();
                ViewBag.Categories = categories;
                ViewBag.Brands = brands;
                return View(product);
            }

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
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
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
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            var category = _context.Categories.Find(productViewModel.CategoryId);
            var product = _context.Products.Find(productViewModel.Id);

            if (product == null)
            {
                return NotFound();
            }

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

            product.IsDeleted = true;
            _context.Products.Update(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult AddToCartSingle(int productId)
        {
            var userId = UserHelper.GetUserId(User);
            var cart = _context.Carts.Where(c => c.UserId == userId && c.IsCheckedOut == false).FirstOrDefault();

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    IsCheckedOut = false
                };
                _context.Carts.Add(cart);
                _context.SaveChanges();
            }

            var product = _context.Products.Find(productId);
            var cartItem = _context.CartItems.Where(ci => ci.CartId == cart.CartId && ci.ProductId == productId).FirstOrDefault();
            if (cartItem != null)
            {
                cartItem.Quantity++;
                _context.CartItems.Update(cartItem);
                _context.SaveChanges();
            }
            else
            {
                cartItem = new CartItem
                {
                    CartId = cart.CartId,
                    Cart = cart,
                    ProductId = productId,
                    Product = product,
                    Quantity = 1
                };
                _context.CartItems.Add(cartItem);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult AddToCartFromDetailsPage(int productId, int quantity)
        {
            var userId = UserHelper.GetUserId(User);
            var cart = _context.Carts.FirstOrDefault(c => c.UserId == userId && !c.IsCheckedOut);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    IsCheckedOut = false
                };
                _context.Carts.Add(cart);
                _context.SaveChanges();
            }

            var product = _context.Products.Find(productId);
            var cartItem = _context.CartItems.FirstOrDefault(ci => ci.CartId == cart.CartId && ci.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
                _context.CartItems.Update(cartItem);
                _context.SaveChanges();
            }
            else
            {
                cartItem = new CartItem
                {
                    CartId = cart.CartId,
                    Cart = cart,
                    ProductId = productId,
                    Product = product,
                    Quantity = quantity
                };
                _context.CartItems.Add(cartItem);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

    }
}
