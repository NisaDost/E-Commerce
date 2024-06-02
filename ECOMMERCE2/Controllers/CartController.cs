<<<<<<< Updated upstream
﻿using Microsoft.AspNetCore.Mvc;
=======
﻿using ECOMMERCE2.Data;
using ECOMMERCE2.Data.Model;
using ECOMMERCE2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
>>>>>>> Stashed changes

namespace ECOMMERCE2.Controllers
{
    public class CartController : Controller
    {
<<<<<<< Updated upstream
=======
        private readonly DBContext _context;
        public CartController(DBContext context)
        {
            _context = context;
        }

>>>>>>> Stashed changes
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Checkout()
        {
<<<<<<< Updated upstream
            return View();
        }
=======
            var billingAddress = _context.BillingAddresses.ToList();
            var billingCard = _context.BillingCard.ToList();

            ViewBag.BillingAddress = billingAddress;
            ViewBag.BillingCard = billingCard;

            return View();
        }

        [HttpPost]
        public IActionResult Checkout(BillingViewModel billingViewModel)
        {
            if (billingViewModel.SaveAddress)
            {
                var billingAddress = new BillingAddress
                {
                    UserId = billingViewModel.UserId,
                    FirstName = billingViewModel.FirstName,
                    LastName = billingViewModel.LastName,
                    Email = billingViewModel.Email,
                    Mobile = billingViewModel.Mobile,
                    Country = billingViewModel.Country,
                    City = billingViewModel.City,
                    Address = billingViewModel.Address
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
                    CVV = billingViewModel.CVV
                };
                _context.BillingCard.Add(billingCard);
            }

            _context.SaveChanges();
            return RedirectToAction("Checkout");
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
>>>>>>> Stashed changes
    }
}
