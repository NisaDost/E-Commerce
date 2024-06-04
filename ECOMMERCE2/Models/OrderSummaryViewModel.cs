namespace ECOMMERCE2.Models
{
    public class OrderSummaryViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
