using ConfigurationDemo.ConfigModels;
using ConfigurationDemo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;

namespace ConfigurationDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfigurationRoot _configuration;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IConfiguration configuration, ILogger<HomeController> logger)
        {
            _configuration = (IConfigurationRoot)configuration;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Providers()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("Configuration Providers");
            foreach (var provider in _configuration.Providers)
            {
                stringBuilder.AppendLine(provider.ToString());
            }
            stringBuilder.AppendLine("\n----------------------------------\n");
            stringBuilder.AppendLine("Configuration Values");
            foreach (var kvp in _configuration.AsEnumerable())
            {
                stringBuilder.AppendLine($"{kvp.Key} = {kvp.Value}");
            }

            return Content(stringBuilder.ToString(), "text/plain");
        }

        [Route("key/{key}")]
        public IActionResult GetValueByKey([FromRoute]string key)
        {
            if (string.IsNullOrEmpty(key)) return BadRequest();

            var value = _configuration[key];

            return string.IsNullOrEmpty(value) ? Content($"Key '{key}' not found.", "text/plain") : Content($"Key: {key}, Value: {value}", "text/plain");
        }

        [Route("api-settings")]
        public IActionResult GetApiSettings()
        {
            var info = _configuration.GetSection("ApiSettings").Get<ApiSettings>();

            if (info is null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            StringBuilder output = new StringBuilder();
            output.AppendLine($"Base Url: {info.BaseUrl}");
            output.AppendLine($"Api Key: {info.ApiKey}");
            output.AppendLine($"Api Secret: {info.ApiSecret}");

            return Content(output.ToString(), "text/plain");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
