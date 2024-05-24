namespace ECOMMERCE2.Data.Model
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }
        public List<Category> Category { get; set; }
        public List<Cart> Cart { get; set; }
    }
}
