using Microsoft.EntityFrameworkCore;
using System;
using ECOMMERCE2.Data.Model;

namespace ECOMMERCE2.Data
{
    public class DBContext : DbContext
    {
        static List<Product> _productList;
        static int productCount;
        public DBContext(DbContextOptions<DBContext> options) : base(options) 
        {
            _productList =
            [
                new Product(){Id=1, Brand = "Converse",  Name ="Chuck Taylor", Picture = "~/img/converse-ad.gif", Price = 75, StockQuantity=20, InStock = true, Description ="LOREM1", /*ProductCategories=""*/ },

                new Product(){Id=2, Brand = "Nike", Name ="Air Jordan", Picture = "~/img/sneaker-4.jpg", Price = 150, StockQuantity=5, InStock = true, Description ="LOREM2", /*ProductCategories=""*/},

             ];
            productCount = _productList.Where(p => p.InStock == true).Count();
        }

        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<BillingDetails> BillingDetails { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCategory>()
                .HasKey(pc => new { pc.ProductId, pc.CategoryId });

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Product)
                .WithMany(p => p.ProductCategories)
                .HasForeignKey(pc => pc.ProductId);

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Category)
                .WithMany(c => c.ProductCategories)
                .HasForeignKey(pc => pc.CategoryId);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany()
                .HasForeignKey(od => od.ProductId);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithOne(u => u.Cart)
                .HasForeignKey<Cart>(c => c.UserId);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");
        }
        public static List<Product> GetProducts()
        {
            return _productList;
        }
        public static int GetProductCount()
        {
            return productCount;
        }
    }
}

