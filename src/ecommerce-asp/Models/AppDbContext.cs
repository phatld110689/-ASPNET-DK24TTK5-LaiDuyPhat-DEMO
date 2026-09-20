using Microsoft.EntityFrameworkCore;

namespace ecommerce_asp.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Laptop> Laptops { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Email)
                .IsUnique();

            modelBuilder.Entity<Laptop>()
                .HasOne(l => l.Category)
                .WithMany(c => c.Laptops)
                .HasForeignKey(l => l.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Laptop>()
                .HasOne(l => l.Brand)
                .WithMany(b => b.Laptops)
                .HasForeignKey(l => l.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Laptop)
                .WithMany(l => l.OrderDetails)
                .HasForeignKey(od => od.LaptopId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Wishlist>()
                .HasOne(w => w.Customer)
                .WithMany(c => c.Wishlists)
                .HasForeignKey(w => w.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Wishlist>()
                .HasOne(w => w.Laptop)
                .WithMany(l => l.Wishlists)
                .HasForeignKey(w => w.LaptopId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Wishlist>()
                .HasIndex(w => new { w.CustomerId, w.LaptopId })
                .IsUnique();

            modelBuilder.Entity<CartItem>()
                .HasOne(c => c.Customer)
                .WithMany(cus => cus.CartItems)
                .HasForeignKey(c => c.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(c => c.Laptop)
                .WithMany(l => l.CartItems)
                .HasForeignKey(c => c.LaptopId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CartItem>()
                .HasIndex(c => new { c.CustomerId, c.LaptopId })
                .IsUnique();
        }
    }
}