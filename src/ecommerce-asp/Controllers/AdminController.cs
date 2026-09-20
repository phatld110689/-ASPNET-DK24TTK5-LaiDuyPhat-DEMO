using ecommerce_asp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ecommerce_asp.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("UserRole") == "Admin";
        }

        public IActionResult Index()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.TotalProducts = _context.Laptops.Count();
            ViewBag.TotalOrders = _context.Orders.Count();
            ViewBag.TotalCustomers = _context.Customers.Count();
            ViewBag.TotalRevenue = _context.Orders.Sum(o => (decimal?)o.TotalAmount) ?? 0;

            return View();
        }

        public IActionResult Products()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            var products = _context.Laptops
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .OrderByDescending(p => p.LaptopId)
                .ToList();

            return View(products);
        }

        [HttpGet]
        public IActionResult CreateProduct()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Brands = new SelectList(_context.Brands.ToList(), "BrandId", "Name");
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "CategoryId", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult CreateProduct(Laptop model)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Brands = new SelectList(_context.Brands.ToList(), "BrandId", "Name", model.BrandId);
                ViewBag.Categories = new SelectList(_context.Categories.ToList(), "CategoryId", "Name", model.CategoryId);
                return View(model);
            }

            _context.Laptops.Add(model);
            _context.SaveChanges();

            return RedirectToAction("Products");
        }

        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            var product = _context.Laptops.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Brands = new SelectList(_context.Brands.ToList(), "BrandId", "Name", product.BrandId);
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "CategoryId", "Name", product.CategoryId);

            return View(product);
        }

        [HttpPost]
        public IActionResult EditProduct(Laptop model)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Brands = new SelectList(_context.Brands.ToList(), "BrandId", "Name", model.BrandId);
                ViewBag.Categories = new SelectList(_context.Categories.ToList(), "CategoryId", "Name", model.CategoryId);
                return View(model);
            }

            _context.Laptops.Update(model);
            _context.SaveChanges();

            return RedirectToAction("Products");
        }

        public IActionResult DeleteProduct(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            var product = _context.Laptops.Find(id);
            if (product != null)
            {
                _context.Laptops.Remove(product);
                _context.SaveChanges();
            }

            return RedirectToAction("Products");
        }

        public IActionResult Orders()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            var orders = _context.Orders
                .Include(o => o.Customer)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }

        [HttpPost]
        public IActionResult UpdateOrderStatus(int id, string status)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            order.Status = status;
            _context.SaveChanges();

            return RedirectToAction("Orders");
        }

        public IActionResult Customers()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            var customers = _context.Customers
                .OrderByDescending(c => c.CustomerId)
                .ToList();

            return View(customers);
        }
    }
}