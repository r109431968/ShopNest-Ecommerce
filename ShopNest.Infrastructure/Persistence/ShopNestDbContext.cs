using Microsoft.EntityFrameworkCore;
using ShopNest.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopNest.Infrastructure.Persistence
{
    public class ShopNestDbContext : DbContext
    {
        public ShopNestDbContext(DbContextOptions<ShopNestDbContext> options)
            : base(options)
        { }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. Product - Category (Many Products belong to One Category)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // 2. User - RefreshToken (One User has Many RefreshTokens)
            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);   // User delete ho to uske tokens bhi chale jayen (tokens ka koi matlab nahi bina user ke)

            // 3. User - Cart (One User has One Cart... but structured as One-to-Many here)
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithMany()                          // User.cs mein humne Cart ka collection nahi banaya, isliye khali
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);   // User delete ho to Cart bhi chala jaye

            // 4. Cart - CartItem (One Cart has Many CartItems)
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);   // Cart delete ho to uske items bhi chale jayen

            // 5. CartItem - Product (Many CartItems can reference One Product)
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany()                          // Product.cs mein CartItem ka collection nahi rakha
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);  // Product delete na ho agar kisi cart mein hai

            // 6. User - Order (One User has Many Orders)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);  // User delete ho bhi jaye, purane Orders record rehne chahiye (business/financial history)

            // 7. Order - OrderItem (One Order has Many OrderItems)
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);   // Order delete ho (rare case) to uske items bhi jayen

            // 8. OrderItem - Product (Many OrderItems can reference One Product)
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);  // Product delete na ho agar kisi order mein hai (history preserve)

            // 9. Product - Review (One Product has Many Reviews)
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Product)
                .WithMany()
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);   // Product delete ho (rare, soft-delete hi hoga) to reviews bhi jayen

            // 10. User - Review (One User has Many Reviews)
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);  // User delete ho bhi jaye, uski di hui reviews record rahe
        }
    }
}
