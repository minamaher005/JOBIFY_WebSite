using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IServiceProvider _serviceProvider;

        public HomeController(ILogger<HomeController> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        // Action to seed the database
        public async Task<IActionResult> SeedDatabase()
        {
            try
            {
                await SeedData.Initialize(_serviceProvider);
                TempData["SuccessMessage"] = "Database seeded successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error seeding database: {ex.Message}";
                if (ex.InnerException != null)
                {
                    TempData["ErrorMessage"] += $" Inner error: {ex.InnerException.Message}";
                }
            }
            
            return RedirectToAction("Index");
        }
    }
}
