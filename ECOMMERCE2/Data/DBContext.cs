using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System;
using ECOMMERCE2.Data.Model;

namespace ECOMMERCE2.Data
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<BillingDetail> BillingDetails { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=localhost; database=EcommerceDB; integrated security=true;");
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<User>().HasOne(u => u.BillingDetail).WithOne(b => b.User).HasForeignKey<User>(u => u.BillingDetail);
            modelBuilder.Entity<User>().Property(u => u.FirstName).HasMaxLength(15).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.LastName).HasMaxLength(15).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Email).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Password).HasMaxLength(15).IsRequired();

            modelBuilder.Entity<Admin>().HasKey(a => a.Id);
            modelBuilder.Entity<Admin>().Property(a => a.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Admin>().Property(a => a.FirstName).HasMaxLength(15).IsRequired();
            modelBuilder.Entity<Admin>().Property(a => a.LastName).HasMaxLength(15).IsRequired();
            modelBuilder.Entity<Admin>().Property(a => a.Email).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Admin>().Property(a => a.Password).HasMaxLength(15).IsRequired();

            modelBuilder.Entity<BillingDetail>().HasKey(b => b.Id);
            modelBuilder.Entity<BillingDetail>().Property(b => b.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<BillingDetail>().Property(b => b.FirstName).HasMaxLength(15).IsRequired();
            modelBuilder.Entity<BillingDetail>().Property(b => b.LastName).HasMaxLength(15).IsRequired();
            modelBuilder.Entity<BillingDetail>().Property(b => b.Email).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<BillingDetail>().Property(b => b.Mobile).HasMaxLength(11).IsRequired();
            modelBuilder.Entity<BillingDetail>().Property(b => b.Country).HasMaxLength(15).IsRequired();
            modelBuilder.Entity<BillingDetail>().Property(b => b.City).HasMaxLength(15).IsRequired();
            modelBuilder.Entity<BillingDetail>().Property(b => b.Address).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<BillingDetail>().Property(b => b.NameOnCard).HasMaxLength(30).IsRequired();
            modelBuilder.Entity<BillingDetail>().Property(b => b.CardNumber).HasMaxLength(16).IsRequired();
            modelBuilder.Entity<BillingDetail>().Property(b => b.ExpiryDate).HasMaxLength(4).IsRequired();
            modelBuilder.Entity<BillingDetail>().Property(b => b.CVV).HasMaxLength(3).IsRequired();

            modelBuilder.Entity<Category>().HasKey(c => c.Id);
            modelBuilder.Entity<Category>().Property(c => c.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Category>().Property(c => c.Name).HasMaxLength(15).IsRequired();

            modelBuilder.Entity<Product>().HasKey(p => p.Id);
            modelBuilder.Entity<Product>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Product>().Property(p => p.Name).HasMaxLength(15).IsRequired();
            modelBuilder.Entity<Product>().Property(p => p.Price).IsRequired();
            modelBuilder.Entity<Product>().Property(p => p.Quantity).IsRequired();
            modelBuilder.Entity<Product>().Property(p => p.Image).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Product>().HasMany(p => p.Category).WithMany(c => c.Product).HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Cart>().HasKey(c => c.Id);
            modelBuilder.Entity<Cart>().Property(c => c.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Cart>().Property(c => c.Quantity).IsRequired();
            modelBuilder.Entity<Cart>().HasOne(c => c.User).WithOne(u => u.Cart).HasForeignKey(c => c.UserId);
            modelBuilder.Entity<Cart>().HasOne(c => c.Product).WithMany(p => p.Cart).HasForeignKey(c => c.Product);
            modelBuilder.Entity<Cart>().HasOne(c => c.BillingDetail).WithOne(b => b.Cart).HasForeignKey(c => c.BillingDetail);


            
            base.OnModelCreating(modelBuilder);
        }
    }
}
