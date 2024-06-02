﻿using ECOMMERCE2.Data;
using ECOMMERCE2.Data.Model;
using ECOMMERCE2.Helper;
using ECOMMERCE2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECOMMERCE2.Controllers
{
    public class CartController : Controller
    {
        private readonly DBContext _context;
        public CartController(DBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var total = 0.00M;
            var shipping = 15.00M;
            ViewBag.Total = total;
            ViewBag.Shipping = shipping;
            var userId = UserHelper.GetUserId(User);
            var cart = _context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Product).FirstOrDefault(c => c.UserId == userId && !c.IsCheckedOut);
            return View(cart);
        }
        public IActionResult Checkout()
        {
            var total = 0.00M;
            ViewBag.Total = total;

            var shipping = 15.00M;
            ViewBag.Shipping = shipping;

            var billingAdress = _context.BillingAddresses.ToList();
            ViewBag.billingAdress = billingAdress;

            var billingCard = _context.BillingCard.ToList();
            ViewBag.billingCard = billingCard;
            
            var userId = UserHelper.GetUserId(User);
            var cart = _context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Product).Where(c => c.UserId == userId && !c.IsCheckedOut).FirstOrDefault();
            ViewBag.Cart = cart;
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(BillingViewModel billingViewModel)
        {
            var userId = UserHelper.GetUserId(User);
            var user = _context.Users.Find(userId);

            var cart = _context.Carts.FirstOrDefault(c => c.UserId == userId && !c.IsCheckedOut);
            cart.IsCheckedOut = true;
            _context.Carts.Update(cart);

            var order = new Order
            {
                UserId = userId,
                User = user,
                OrderDate = System.DateTime.Now
            };
            _context.Orders.Add(order);
            _context.SaveChanges();

            var cartItems = _context.CartItems.Where(ci => ci.CartId == cart.CartId).ToList();
            foreach (var cartItem in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.OrderId,
                    Order = order,
                    ProductId = cartItem.ProductId,
                    Product = cartItem.Product,
                    Quantity = cartItem.Quantity
                };
                _context.OrderDetails.Add(orderDetail);

                _context.Products.Find(cartItem.ProductId).StockQuantity -= cartItem.Quantity;
            }

            if (billingViewModel.SaveAddress)
            {
                var billingAddress = new BillingAddress
                {
                    FirstName = billingViewModel.FirstName,
                    LastName = billingViewModel.LastName,
                    Email = billingViewModel.Email,
                    Mobile = billingViewModel.Mobile,
                    Country = billingViewModel.Country,
                    City = billingViewModel.City,
                    Address = billingViewModel.Address,
                    UserId = userId,
                    User = user,
                    OrderId = order.OrderId,
                    Order = order
                };
                _context.BillingAddresses.Add(billingAddress);
            }

            if (billingViewModel.SaveCard)
            {
                var billingCard = new BillingCard
                {
                    CreditCardNumber = billingViewModel.CreditCardNumber,
                    CardHolderName = billingViewModel.CardHolderName,
                    ExpiryDateMonth = billingViewModel.ExpiryDateMonth,
                    ExpiryDateYear = billingViewModel.ExpiryDateYear,
                    CVV = billingViewModel.CVV,
                    UserId = userId,
                    User = user,
                    OrderId = order.OrderId,
                    Order = order
                };
                _context.BillingCard.Add(billingCard);
            }

            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult GetAddress(int id)
        {
            var billingAddress = _context.BillingAddresses.Find(id);
            return View(billingAddress);
        }

        [HttpGet]
        public IActionResult GetCard(int id)
        {
            var billingCard = _context.BillingCard.Find(id);
            return View(billingCard);
        }

        public IActionResult Order(OrderViewModel orderViewModel)
        {
            var orderDetails = _context.OrderDetails.ToList();
            ViewBag.orderDetails = orderDetails;
            
            return View(orderViewModel);
        }

        [HttpPost]
        public IActionResult EditCartItemQuantity(int cartItemId, string editType)
        {
            var cartItem = _context.CartItems.FirstOrDefault(ci => ci.CartItemId == cartItemId);
            if (editType == "increase")
            {
                cartItem.Quantity++;
            }
            else
            {
                cartItem.Quantity--;
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}