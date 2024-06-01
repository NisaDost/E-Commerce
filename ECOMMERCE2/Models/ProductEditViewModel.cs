﻿namespace ECOMMERCE2.Models
{
    public class ProductEditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool InStock { get; set; }
        public int CategoryId { get; set; }
        public IFormFile Image { get; set; }
    }
}
