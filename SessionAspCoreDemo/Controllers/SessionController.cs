using Microsoft.AspNetCore.Mvc;

namespace SessionAspCoreDemo.Controllers
{
    public class SessionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
