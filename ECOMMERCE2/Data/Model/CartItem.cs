using System.ComponentModel.DataAnnotations;
namespace ECOMMERCE2.Data.Model
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
