using System.ComponentModel.DataAnnotations;

namespace ECOMMERCE2.Models
{
    public class BillingViewModel
    {
        public int UserId { get; set; }
        // Address
        public int BillingAddressId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mobile number is required")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
        public bool SaveAddress { get; set; }

        // Card
        public int BillingCardId { get; set; }

        [Required(ErrorMessage = "Card Holder name is required")]
        public string CardHolderName { get; set; }

        [Required(ErrorMessage = "Credit card number is required")]
        [CreditCard(ErrorMessage = "Invalid credit card number")]
        public string CreditCardNumber { get; set; }

        [Required(ErrorMessage = "Expiry month is required")]
        [Range(1, 12, ErrorMessage = "Invalid month")]
        public int ExpiryDateMonth { get; set; }

        [Required(ErrorMessage = "Expiry year is required")]
        [Range(0, 99, ErrorMessage = "Invalid year")]
        public int ExpiryDateYear { get; set; }

        [Required(ErrorMessage = "CVV is required")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Invalid CVV")]
        public string CVV { get; set; }
        public bool SaveCard { get; set; }
    }
}
