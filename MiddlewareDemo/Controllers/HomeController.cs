using Microsoft.AspNetCore.Mvc;
using MiddlewareDemo.MiddlewareByHoang;
using MiddlewareDemo.Models;
using System.Diagnostics;

namespace MiddlewareDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IKeyData _keyData;

        public HomeController(IKeyData keyData, ILogger<HomeController> logger)
        {
            _logger = logger;
            _keyData = keyData;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("key")]
        public IActionResult GetUserByApiKey([FromQuery]string keyApi)
        {
            return Json(_keyData.GetApiKeyData().ToList().Find(x => x.ApiKey == keyApi));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
