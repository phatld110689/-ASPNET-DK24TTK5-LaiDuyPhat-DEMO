using ecommerce_asp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ecommerce_asp.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string keyword)
        {
            var query = _context.Laptops
                .Include(l => l.Brand)
                .Include(l => l.Category)
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(l => l.Name.Contains(keyword));
            }

            var laptops = query.ToList();

            return View(laptops);
        }

        public IActionResult Detail(int id)
        {
            var laptop = _context.Laptops
                .Include(l => l.Brand)
                .Include(l => l.Category)
                .FirstOrDefault(l => l.LaptopId == id);

            if (laptop == null)
            {
                return NotFound();
            }

            var relatedProducts = _context.Laptops
                .Include(l => l.Brand)
                .Include(l => l.Category)
                .Where(l => l.CategoryId == laptop.CategoryId && l.LaptopId != laptop.LaptopId)
                .Take(4)
                .ToList();

            ViewBag.RelatedProducts = relatedProducts;

            return View(laptop);
        }
    }
}