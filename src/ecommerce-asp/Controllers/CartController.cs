using ecommerce_asp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ecommerce_asp.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
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

            return View(cartItems);
        }

       
        public IActionResult Add(int id, int quantity = 1)
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerId");
            if (customerId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (quantity < 1)
            {
                quantity = 1;
            }

            var laptop = _context.Laptops.AsNoTracking().FirstOrDefault(l => l.LaptopId == id);
            if (laptop == null)
            {
                return NotFound();
            }

            var existingCartItem = _context.CartItems
                .AsNoTracking()
                .FirstOrDefault(c => c.CustomerId == customerId.Value && c.LaptopId == id);

            if (existingCartItem != null)
            {
                var cartItemToUpdate = _context.CartItems
                    .FirstOrDefault(c => c.CartItemId == existingCartItem.CartItemId);

                if (cartItemToUpdate != null)
                {
                    cartItemToUpdate.Quantity += quantity;
                }
            }
            else
            {
                var newCartItem = new CartItem
                {
                    CustomerId = customerId.Value,
                    LaptopId = id,
                    Quantity = quantity
                };

                _context.CartItems.Add(newCartItem);
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Increase(int id)
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerId");
            if (customerId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cartItem = _context.CartItems
                .FirstOrDefault(c => c.CartItemId == id && c.CustomerId == customerId.Value);

            if (cartItem != null)
            {
                cartItem.Quantity += 1;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Decrease(int id)
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerId");
            if (customerId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cartItem = _context.CartItems
                .FirstOrDefault(c => c.CartItemId == id && c.CustomerId == customerId.Value);

            if (cartItem != null)
            {
                cartItem.Quantity -= 1;

                if (cartItem.Quantity <= 0)
                {
                    _context.CartItems.Remove(cartItem);
                }

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerId");
            if (customerId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cartItem = _context.CartItems
                .FirstOrDefault(c => c.CartItemId == id && c.CustomerId == customerId.Value);

            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}