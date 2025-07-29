using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagement.WebApp.Data;
using UserManagement.WebApp.Models;
using UserManagement.WebApp.Filters;

namespace UserManagement.WebApp.Controllers
{
    [AuthorizeUser]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Users/Index or /Users
        // This will be our main user dashboard page.
        public async Task<IActionResult> Index()
        {
            // Per HR: "DATA IN THE TABLE SHOULD BE SORTED (E.G., BY THE LAST LOGIN TIME)."
            var users = await _context.Users
                .OrderByDescending(u => u.LastLoginTimeUtc)
                .ToListAsync();

            var viewModel = new UserTableViewModel
            {
                Users = users
            };

            return View(viewModel);
        }
        // File: Controllers/UsersController.cs - FINAL VERSIONS

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Block(List<int> userIds) // The parameter comes from the form now
        {
            if (userIds == null || !userIds.Any())
            {
                return RedirectToAction("Index");
            }

            var usersToBlock = await _context.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            foreach (var user in usersToBlock)
            {
                user.IsBlocked = true;
            }
            await _context.SaveChangesAsync();

            // Set a status message to inform the user of the action taken.
            TempData["StatusMessage"] = $"success:{userIds.Count} user(s) have been blocked.";


            var currentUserId = HttpContext.Session.GetInt32("UserId");
            if (currentUserId.HasValue && userIds.Contains(currentUserId.Value))
            {
                // If the current user blocked themselves, log them out.
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "Account");
            }

            // Standard redirect back to the user list.
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unblock(List<int> userIds)
        {
            if (userIds == null || !userIds.Any())
            {
                return RedirectToAction("Index");
            }

            var usersToUnblock = await _context.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            foreach (var user in usersToUnblock)
            {
                user.IsBlocked = false;
            }
            await _context.SaveChangesAsync();

            // Set a status message to inform the user of the action taken.
            TempData["StatusMessage"] = $"success:{userIds.Count} user(s) have been unblocked.";

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(List<int> userIds)
        {
            if (userIds == null || !userIds.Any())
            {
                return RedirectToAction("Index");
            }

            var usersToDelete = await _context.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            var currentUserId = HttpContext.Session.GetInt32("UserId");
            bool isCurrentUserDeleted = currentUserId.HasValue && userIds.Contains(currentUserId.Value);

            _context.Users.RemoveRange(usersToDelete);
            await _context.SaveChangesAsync();

            // Set a status message to inform the user of the action taken.
            TempData["StatusMessage"] = $"success:{userIds.Count} user(s) have been deleted.";


            if (isCurrentUserDeleted)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "Account");
            }

            return RedirectToAction("Index");
        }
    }
}
