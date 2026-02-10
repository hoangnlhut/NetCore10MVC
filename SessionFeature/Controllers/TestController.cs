using Microsoft.AspNetCore.Mvc;

namespace SessionFeature.Controllers
{
    public class TestController : Controller
    {
        public IActionResult TestGetSession()
        {
            var session = HttpContext.GetSession();
            session.SetString("Name", "1234567");

            var newSession = HttpContext.GetSession();
            var value = newSession.GetString("Name");

            if (value == "1234567")
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
