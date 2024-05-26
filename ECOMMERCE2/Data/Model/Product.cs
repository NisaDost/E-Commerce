using System.ComponentModel.DataAnnotations;

namespace ECOMMERCE2.Data.Model
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Picture { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int StockQuantity { get; set; }
        public string Description { get; set; }
        public ICollection<ProductCategory> ProductCategories { get; set; }
    }
}
