namespace ECOMMERCE2.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductBrand { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductImage { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
