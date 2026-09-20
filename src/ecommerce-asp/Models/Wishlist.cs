using System.ComponentModel.DataAnnotations;

namespace ecommerce_asp.Models
{
    public class Wishlist
    {
        [Key]
        public int WishlistId { get; set; }

        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public int LaptopId { get; set; }
        public Laptop? Laptop { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}