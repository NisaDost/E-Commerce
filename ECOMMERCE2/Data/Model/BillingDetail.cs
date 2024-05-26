using System.ComponentModel.DataAnnotations;

namespace ECOMMERCE2.Data.Model
{
    public class BillingDetails
    {
        [Key]
        public int BillingDetailsId { get; set; }
        [Required]
        public string CreditCardNumber { get; set; }
        [Required]
        public string CardHolderName { get; set; }
        [Required]
        public DateTime ExpiryDate { get; set; }
        [Required]
        public string CVV { get; set; }
    }
}
