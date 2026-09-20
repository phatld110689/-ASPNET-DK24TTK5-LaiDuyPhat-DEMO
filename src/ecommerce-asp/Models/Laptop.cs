using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ecommerce_asp.Models
{
    public class Laptop
    {
        [Key]
        public int LaptopId { get; set; }

        [Required(ErrorMessage = "Tên laptop không được để trống")]
        [StringLength(200)]
        public string Name { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [StringLength(255)]
        public string? ImageUrl { get; set; }

        public string? Description { get; set; }

        public int StockQuantity { get; set; }

        public int Rate { get; set; } = 5;

        [StringLength(100)]
        public string? CPU { get; set; }

        [StringLength(50)]
        public string? RAM { get; set; }

        [StringLength(50)]
        public string? Storage { get; set; }

        [StringLength(100)]
        public string? Screen { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public int BrandId { get; set; }
        public Brand? Brand { get; set; }

        public List<OrderDetail>? OrderDetails { get; set; }
        public List<Wishlist>? Wishlists { get; set; }
        public List<CartItem>? CartItems { get; set; }
    }
}