using ECOMMERCE2.Data;
using ECOMMERCE2.Data.Model;
using ECOMMERCE2.Helper;
using ECOMMERCE2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ECOMMERCE2.Controllers
{
    public class OrderController : Controller
    {
        private readonly DBContext _context;
        public OrderController(DBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            var orders = _context.Orders.Include(o => o.OrderDetails).ThenInclude(p => p.Product).Include(o => o.User).ToList();

            var orderViewModels = orders.Select(o => new AdminOrderViewModel
            {
                Order = o,
                TotalPrice = o.OrderDetails.Sum(od => od.Product.Price * od.Quantity)
            }).ToList();

            return View(orderViewModels);
        }

        public IActionResult UserOrder()
        {
            var user = UserHelper.GetUserId(User);
            var orders = _context.Orders.Include(o => o.OrderDetails).ThenInclude(p => p.Product).Where(o => o.UserId == user).ToList();

            return View(orders);
        }
        [HttpGet]
        public IActionResult InsideOrderDetails(/*int id*/)
        {
            //var order = _context.Orders.Find(id);
            return View(/*order*/);
        }
    }
}