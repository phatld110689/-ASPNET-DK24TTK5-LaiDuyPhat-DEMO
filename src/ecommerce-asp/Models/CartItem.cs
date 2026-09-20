using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ecommerce_asp.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }

        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public int LaptopId { get; set; }
        public Laptop? Laptop { get; set; }

        public int Quantity { get; set; }

        [NotMapped]
        public decimal TotalPrice
        {
            get
            {
                return (Laptop != null ? Laptop.Price : 0) * Quantity;
            }
        }
    }
}