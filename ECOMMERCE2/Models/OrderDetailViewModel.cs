namespace ECOMMERCE2.Models
{
    public class OrderDetailViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public string ShippingAddress { get; set; }
        public List<OrderViewModel> OrderItems { get; set; }
        public decimal Total { get; set; }
    }
}
