using ecommerce_asp.Models;
using ecommerce_asp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ecommerce_asp.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerId");
            if (customerId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cartItems = _context.CartItems
                .Include(c => c.Laptop)
                .Where(c => c.CustomerId == customerId.Value)
                .ToList();

            if (!cartItems.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == customerId.Value);

            var model = new CheckoutViewModel
            {
                ReceiverName = customer?.FullName ?? "",
                PhoneNumber = customer?.PhoneNumber ?? "",
                Address = customer?.Address ?? "",
                CartItems = cartItems,
                TotalAmount = cartItems.Sum(c => c.TotalPrice)
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Checkout(CheckoutViewModel model)
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerId");
            if (customerId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cartItems = _context.CartItems
                .Include(c => c.Laptop)
                .Where(c => c.CustomerId == customerId.Value)
                .ToList();

            if (!cartItems.Any())
            {
                ModelState.AddModelError("", "Giỏ hàng đang trống.");
            }

            if (!ModelState.IsValid)
            {
                model.CartItems = cartItems;
                model.TotalAmount = cartItems.Sum(c => c.TotalPrice);
                return View(model);
            }

            var order = new Order
            {
                CustomerId = customerId.Value,
                ReceiverName = model.ReceiverName,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                Note = model.Note,
                TotalAmount = cartItems.Sum(c => c.TotalPrice),
                Status = "Chờ xác nhận",
                OrderDate = DateTime.Now
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.OrderId,
                    LaptopId = item.LaptopId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Laptop != null ? item.Laptop.Price : 0
                };

                _context.OrderDetails.Add(orderDetail);
            }

            _context.SaveChanges();

            var cartItemsToRemove = _context.CartItems
                .Where(c => c.CustomerId == customerId.Value)
                .ToList();

            _context.CartItems.RemoveRange(cartItemsToRemove);
            _context.SaveChanges();

            TempData["SuccessOrder"] = "Đặt hàng thành công!";
            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult MyOrders()
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerId");
            if (customerId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var orders = _context.Orders
                .Where(o => o.CustomerId == customerId.Value)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }
        public IActionResult Detail(int id)
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerId");
            if (customerId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var order = _context.Orders
                .Include(o => o.OrderDetails!)
                    .ThenInclude(od => od.Laptop)
                .FirstOrDefault(o => o.OrderId == id && o.CustomerId == customerId.Value);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}