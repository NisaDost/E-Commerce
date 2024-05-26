using System.ComponentModel.DataAnnotations;

namespace ECOMMERCE2.Data.Model
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime OrderDate { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        [Required]
        public string ShippingAddress { get; set; }
        [Required]
        public string BillingInformation { get; set; }
    }
}
