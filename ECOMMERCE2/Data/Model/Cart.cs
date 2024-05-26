using System.ComponentModel.DataAnnotations;

namespace ECOMMERCE2.Data.Model
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}
