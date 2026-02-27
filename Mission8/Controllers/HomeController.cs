// =============================================================================
// HomeController.cs – Home Page Controller
// Handles requests to the landing page and static pages (Home, Privacy, Error).
// This is the default controller the app routes to when no controller is
// specified in the URL (configured in Program.cs routing).
// =============================================================================

using Microsoft.AspNetCore.Mvc;
using Mission8.Models;
using System.Diagnostics;

namespace Mission8.Controllers
{
    public class HomeController : Controller
    {
        // GET: /Home/Index (or just /)
        // Displays the main landing page with navigation buttons
        // to "View Tasks" and "Add Task"
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Home/Privacy
        // Displays the privacy policy page (default ASP.NET Core scaffold)
        public IActionResult Privacy()
        {
            return View();
        }

        // GET: /Home/Error
        // Displays a user-friendly error page when an unhandled exception occurs.
        // ResponseCache attributes prevent the error page from being cached,
        // ensuring fresh error info is always shown.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
