using ECOMMERCE2.Data.Model;

namespace ECOMMERCE2.Models
{
    public class AdminOrderViewModel
    {
        public Order Order { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
