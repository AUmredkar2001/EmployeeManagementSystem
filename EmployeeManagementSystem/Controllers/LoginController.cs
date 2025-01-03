using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeManagementSystem.Controllers
{
    public class LoginController : Controller
    {
        private readonly EmployeeDbContext _context;
        private readonly IConfiguration _configuration;

        public LoginController(EmployeeDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string username, string password)
        {
            var user = _context.Authentications.FirstOrDefault(u => u.UserName == username && u.Password == password);
            if (user == null)
            {
                ViewBag.Message = "Invalid Credential.";
                return View();
            }
            var token = GenerateToken(user);
            TempData["Token"] = token;
            return RedirectToAction("Index", "Employee");
        }

        private string GenerateToken(Authentication authentication)
        {
            var claim = new[]
            {
                new Claim(ClaimTypes.Name, authentication.UserName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTSettings:SecretKey"]));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claim,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: cred);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Authentication authentication)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Authentications.Add(authentication);
                _context.SaveChanges();
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        [HttpGet]
        public IActionResult HomePage()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            return RedirectToAction("HomePage");
        }
    }
}
