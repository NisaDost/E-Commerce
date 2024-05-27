using System.ComponentModel.DataAnnotations;

namespace ECOMMERCE2.Data.Model
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<BillingCard> BillingCards { get; set; }
        public ICollection<BillingAddress> BillingAddresses { get; set; }
        public Cart Cart { get; set; }
    }
}
