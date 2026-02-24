using Microsoft.AspNetCore.Mvc;

namespace SessionAspCoreDemo.Controllers
{
    public class SessionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SetSession(string key, string value)
        {
            HttpContext.Session.SetString(key, value);
            return Ok($"Session Saved {key} and {value}");
        }
        public IActionResult GetSession(string key)
        {
            string? value = HttpContext.Session.GetString(key);
            value = string.IsNullOrEmpty(value) ? "Unknown Key" : value;
            return Ok($"Value of Session: {value}");
        }
    }
}
