using ECOMMERCE2.Data.Model;
using System.ComponentModel.DataAnnotations;

namespace ECOMMERCE2.Models
{
    public class BillingViewModel
    {
        //address
        public int BillingAddressId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public bool SaveAddress { get; set; }


        //card

        public int BillingCardId { get; set; }
        public string CreditCardNumber { get; set; }
        public string CardHolderName { get; set; }
        public int ExpiryDateMonth { get; set; }
        public int ExpiryDateYear { get; set; }
        public string CVV { get; set; }
        public bool SaveCard { get; set; }

    }
}
