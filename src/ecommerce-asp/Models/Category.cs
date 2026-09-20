using System.ComponentModel.DataAnnotations;

namespace ecommerce_asp.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [StringLength(100)]
        public string Name { get; set; }

        public List<Laptop>? Laptops { get; set; }
    }
}