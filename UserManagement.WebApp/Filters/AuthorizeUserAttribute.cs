using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UserManagement.WebApp.Data;
using Microsoft.EntityFrameworkCore;

namespace UserManagement.WebApp.Filters
{
    public class AuthorizeUserAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Get the current user's ID from the session.
            var userId = context.HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                // If there's no UserId in the session, they are not logged in.
                // Redirect them to the Login page.
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            // Get the database context from the services container.
            var dbContext = context.HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();

            // Check if the user exists in the database and is not blocked.
            var user = await dbContext.Users
                .AsNoTracking() // Use AsNoTracking for a read-only query for performance.
                .FirstOrDefaultAsync(u => u.Id == userId.Value);

            if (user == null || user.IsBlocked)
            {
                // If the user does not exist or is blocked, clear the session and redirect to Login.
                context.HttpContext.Session.Clear();
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            // If the user is valid, proceed to execute the controller action.
            await next();
        }
    }
}
