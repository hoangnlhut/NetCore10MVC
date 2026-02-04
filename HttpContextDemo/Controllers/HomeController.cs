using HttpContextDemo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using HttpContextDemo.Extentions;

namespace HttpContextDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Handling Index request. TraceIdentifier: {TraceId} - request:  {ri}", HttpContext.TraceIdentifier, HttpContext.GetDebugInfo());
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
    }
}
