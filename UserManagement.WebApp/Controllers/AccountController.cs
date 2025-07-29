using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using UserManagement.WebApp.Data;
using UserManagement.WebApp.Models;

namespace UserManagement.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Account/Register
        // This action displays the registration form.
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        // This action processes the submitted registration data.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                RegistrationTimeUtc = DateTime.UtcNow,
                LastLoginTimeUtc = DateTime.UtcNow,
                IsBlocked = false
            };

            _context.Add(user);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlEx && (sqlEx.Number == 2627 || sqlEx.Number == 2601))
                {
                    // This error means the email is already taken.
                    ModelState.AddModelError("Email", "This email address is already registered.");
                    return View(model); // Return the view with the specific error.
                }
                else
                {
                    throw;
                }
            }

            TempData["SuccessMessage"] = "Registration successful! Please log in.";
            return RedirectToAction("Login", "Account");
        }


        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            // This will display our login form view
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == model.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                // For security, use a generic error message.
                // Don't tell the attacker whether the username or password was wrong.
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            if (user.IsBlocked)
            {
                ModelState.AddModelError(string.Empty, "This account has been blocked.");
                return View(model);
            }

            // --- Login Successful ---

            // Update the last login time
            user.LastLoginTimeUtc = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // Store user info in the session to "remember" them.
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.Name);

            // We will create the Users/Index dashboard later. For now, redirect home.
            return RedirectToAction("Index", "Users");
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clear the session data
            return RedirectToAction("Login", "Account");
        }
    }
}
