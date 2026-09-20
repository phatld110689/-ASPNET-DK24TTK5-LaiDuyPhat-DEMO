using ecommerce_asp.Models;
using ecommerce_asp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce_asp.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingCustomer = _context.Customers.FirstOrDefault(c => c.Email == model.Email);
            if (existingCustomer != null)
            {
                ModelState.AddModelError("Email", "Email này đã tồn tại");
                return View(model);
            }

            var customer = new Customer
            {
                FullName = model.FullName,
                Email = model.Email,
                Password = model.Password,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address
            };

            _context.Customers.Add(customer);
            _context.SaveChanges();

            TempData["Success"] = "Đăng ký thành công. Bạn hãy đăng nhập.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var customer = _context.Customers.FirstOrDefault(c =>
                c.Email == model.Email && c.Password == model.Password);

            if (customer == null)
            {
                ViewBag.Error = "Email hoặc mật khẩu không đúng";
                return View(model);
            }

            HttpContext.Session.SetInt32("CustomerId", customer.CustomerId);
            HttpContext.Session.SetString("CustomerName", customer.FullName);
            HttpContext.Session.SetString("CustomerEmail", customer.Email);
            HttpContext.Session.SetString("UserRole", customer.Role ?? "Customer");

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}