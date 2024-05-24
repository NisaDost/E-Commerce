namespace ECOMMERCE2.Data.Model
{
    public class Cart
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public User User { get; set; }
        public Product Product { get; set; }
        public BillingDetail BillingDetail { get; set; }
    }
}
