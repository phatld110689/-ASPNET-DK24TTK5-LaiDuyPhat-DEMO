using System.ComponentModel.DataAnnotations;

namespace ecommerce_asp.Models
{
    public class Brand
    {
        [Key]
        public int BrandId { get; set; }

        [Required(ErrorMessage = "Tên hãng không được để trống")]
        [StringLength(100)]
        public string Name { get; set; }

        public List<Laptop>? Laptops { get; set; }
    }
}