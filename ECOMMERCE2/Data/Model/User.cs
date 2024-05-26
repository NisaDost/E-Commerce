﻿using System.ComponentModel.DataAnnotations;

namespace ECOMMERCE2.Data.Model
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public ICollection<Order> Orders { get; set; }
        public Cart Cart { get; set; }
    }
}
