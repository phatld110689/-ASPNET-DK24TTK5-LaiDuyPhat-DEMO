using ecommerce_asp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ecommerce_asp.Controllers
{
    public class WishlistController : Controller
    {
        private readonly AppDbContext _context;

        public WishlistController(AppDbContext context)
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

            var wishlists = _context.Wishlists
                .Include(w => w.Laptop)
                .ThenInclude(l => l!.Brand)
                .Where(w => w.CustomerId == customerId.Value)
                .OrderByDescending(w => w.CreatedAt)
                .ToList();

            return View(wishlists);
        }

        public IActionResult Add(int id)
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerId");
            if (customerId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var laptop = _context.Laptops.FirstOrDefault(l => l.LaptopId == id);
            if (laptop == null)
            {
                return NotFound();
            }

            var existingWishlist = _context.Wishlists
                .SingleOrDefault(w => w.CustomerId == customerId.Value && w.LaptopId == id);

            if (existingWishlist == null)
            {
                var wishlist = new Wishlist
                {
                    CustomerId = customerId.Value,
                    LaptopId = id,
                    CreatedAt = DateTime.Now
                };

                _context.Wishlists.Add(wishlist);
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

            var wishlist = _context.Wishlists
                .FirstOrDefault(w => w.WishlistId == id && w.CustomerId == customerId.Value);

            if (wishlist != null)
            {
                _context.Wishlists.Remove(wishlist);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Toggle(int id)
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerId");
            if (customerId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var laptop = _context.Laptops.FirstOrDefault(l => l.LaptopId == id);
            if (laptop == null)
            {
                return NotFound();
            }

            var existingWishlist = _context.Wishlists
                .AsNoTracking()
                .SingleOrDefault(w => w.CustomerId == customerId.Value && w.LaptopId == id);

            if (existingWishlist == null)
            {
                var wishlist = new Wishlist
                {
                    CustomerId = customerId.Value,
                    LaptopId = id,
                    CreatedAt = DateTime.Now
                };

                _context.Wishlists.Add(wishlist);
            }
            else
            {
                var wishlistToRemove = _context.Wishlists
                    .FirstOrDefault(w => w.WishlistId == existingWishlist.WishlistId);

                if (wishlistToRemove != null)
                {
                    _context.Wishlists.Remove(wishlistToRemove);
                }
            }

            _context.SaveChanges();

            var referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer))
            {
                return Redirect(referer);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}