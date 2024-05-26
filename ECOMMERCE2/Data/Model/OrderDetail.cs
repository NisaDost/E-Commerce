using System.ComponentModel.DataAnnotations;

namespace ECOMMERCE2.Data.Model
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
