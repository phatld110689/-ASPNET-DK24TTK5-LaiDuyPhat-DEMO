using ecommerce_asp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ecommerce_asp.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string keyword, int? categoryId, int? brandId, int page = 1)
        {
            int pageSize = 6;

            var query = _context.Laptops
                .Include(l => l.Brand)
                .Include(l => l.Category)
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(l => l.Name.Contains(keyword));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(l => l.CategoryId == categoryId.Value);
            }

            if (brandId.HasValue)
            {
                query = query.Where(l => l.BrandId == brandId.Value);
            }

            int totalProducts = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            var laptops = query
                .OrderBy(l => l.LaptopId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Brands = _context.Brands.ToList();

            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.SelectedBrandId = brandId;
            ViewBag.Keyword = keyword;

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(laptops);
        }
    }
}