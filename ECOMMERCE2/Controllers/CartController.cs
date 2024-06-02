using ECOMMERCE2.Data;
using ECOMMERCE2.Data.Model;
using ECOMMERCE2.Models;
using Microsoft.AspNetCore.Mvc;

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
            return View();
        }
        public IActionResult Checkout()
        {
            var billingAdress = _context.BillingAddresses.ToList();
            ViewBag.billingAdress = billingAdress;

            var billingCard = _context.BillingCard.ToList();
            ViewBag.billingCard = billingCard;
            return View();
        }
        [HttpPost]
        public IActionResult SaveAddress(BillingViewModel billingViewModel)
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
    }
}
