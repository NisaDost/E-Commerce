﻿using System.ComponentModel.DataAnnotations;

namespace ECOMMERCE2.Data.Model
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
