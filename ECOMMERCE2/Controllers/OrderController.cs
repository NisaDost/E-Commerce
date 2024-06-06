using ECOMMERCE2.Data;
using ECOMMERCE2.Data.Model;
using ECOMMERCE2.Helper;
using ECOMMERCE2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECOMMERCE2.Controllers
{
    public class OrderController : Controller
    {
        private readonly DBContext _context;
        public OrderController(DBContext context)
        {
            _context = context;
        }

        public IActionResult Index(string customerName, decimal? minTotal, decimal? maxTotal, DateTime? fromDate, DateTime? toDate)
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            var ordersQuery = _context.Orders.Include(o => o.OrderDetails).ThenInclude(p => p.Product).Include(o => o.User).AsQueryable();

            if (!string.IsNullOrEmpty(customerName))
            {
                ordersQuery = ordersQuery.Where(o => o.User.Name.Contains(customerName) || o.User.Surname.Contains(customerName));
            }

            if (minTotal.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.OrderDetails.Sum(od => od.Product.Price * od.Quantity) >= minTotal.Value);
            }

            if (maxTotal.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.OrderDetails.Sum(od => od.Product.Price * od.Quantity) <= maxTotal.Value);
            }

            if (fromDate.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.OrderDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.OrderDate <= toDate.Value);
            }

            var orders = ordersQuery.ToList();

            var orderViewModels = orders.Select(o => new AdminOrderViewModel
            {
                Order = o,
                TotalPrice = o.OrderDetails.Sum(od => od.Product.Price * od.Quantity)
            }).ToList();

            return View(orderViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] OrderDeleteRequest request)
        {
            if (!User.IsInRole("Admin"))
            {
                return Unauthorized();
            }

            // Log the received data for debugging
            Console.WriteLine("Received Order ID: " + request.Id);

            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Include(o => o.BillingAddresses)
                .FirstOrDefaultAsync(o => o.OrderId == request.Id);

            if (order == null)
            {
                return Json(new { success = false, message = "Order not found" });
            }

            // Remove related billing addresses
            _context.BillingAddresses.RemoveRange(order.BillingAddresses);

            // Remove the order details
            _context.OrderDetails.RemoveRange(order.OrderDetails);

            // Remove the order
            _context.Orders.Remove(order);

            try
            {
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error deleting order: " + ex.Message });
            }
        }
        public class OrderDeleteRequest
        {
            public int Id { get; set; }
        }

        public IActionResult UserOrderDetail()
        {
            var userId = UserHelper.GetUserId(User);
            var orders = _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Where(o => o.UserId == userId)
                .Select(o => new OrderSummaryViewModel
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate,
                    TotalPrice = o.OrderDetails.Sum(od => od.Product.Price * od.Quantity)
                })
                .ToList();
            orders.Reverse();
            return View(orders);
        }

        [HttpGet]
        public IActionResult InsideOrderDetails(int id)
        {
            var order = _context.Orders
                .Include(o => o.User)
                .Include(o => o.BillingAddresses)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefault(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            var orderDetails = order.OrderDetails
                .Select(od => new OrderViewModel
                {
                    Id = od.ProductId,
                    ProductName = od.Product.Name,
                    ProductBrand = od.Product.Brand,
                    ProductPrice = od.Product.Price,
                    ProductImage = od.Product.Picture,
                    Quantity = od.Quantity,
                    TotalPrice = (od.Product.Price * od.Quantity) + 15 // Add a fixed shipping cost
                })
                .ToList();

            var orderDetailViewModel = new OrderDetailViewModel
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                CustomerName = order.User.Name,
                CustomerSurname = order.User.Surname,
                ShippingAddress = string.Join(", ", order.BillingAddresses.Select(b => b.Address)),
                OrderItems = orderDetails,
                Total = orderDetails.Sum(od => od.TotalPrice) // Perform the sum operation in memory
            };

            return View(orderDetailViewModel);
        }
    }
}
