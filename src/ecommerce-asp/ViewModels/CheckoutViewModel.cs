using System.ComponentModel.DataAnnotations;
using ecommerce_asp.Models;

namespace ecommerce_asp.ViewModels
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Tên người nhận không được để trống")]
        public string ReceiverName { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        public string Address { get; set; }

        public string? Note { get; set; }

        public List<CartItem>? CartItems { get; set; }
        public decimal TotalAmount { get; set; }
    }
}