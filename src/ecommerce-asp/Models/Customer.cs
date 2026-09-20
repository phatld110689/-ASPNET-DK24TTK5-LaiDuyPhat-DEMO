using System.ComponentModel.DataAnnotations;

namespace ecommerce_asp.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Họ tên không được để trống")]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string Password { get; set; }

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [StringLength(255)]
        public string? Address { get; set; }

        [StringLength(20)]
        public string Role { get; set; } = "Customer";

        public List<Order>? Orders { get; set; }
        public List<Wishlist>? Wishlists { get; set; }
        public List<CartItem>? CartItems { get; set; }
    }
}