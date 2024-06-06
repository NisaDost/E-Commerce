using System.ComponentModel.DataAnnotations;

namespace ECOMMERCE2.Data.Model
{
    public class BillingCard
    {
        [Key]
        public int BillingCardId { get; set; }
        [Required]
        public string CreditCardNumber { get; set; }
        [Required]
        public string CardHolderName { get; set; }
        [Required]
        public int ExpiryDateMonth { get; set; }
        [Required]
        public int ExpiryDateYear { get; set; }
        [Required]
        public string CVV { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public Order Order { get; set; }
        public bool IsCardSaved { get; set; }
    }
}
