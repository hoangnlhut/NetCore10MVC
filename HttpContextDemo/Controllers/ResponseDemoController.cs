using Microsoft.AspNetCore.Mvc;

namespace HttpContextDemo.Controllers
{
    public class ResponseDemoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Test1() 
        { 
            return Content("This is a plain text response from Test1 action.");
        }

        public IActionResult Test2()
        {
            return Ok("Test2");
        }

        public IActionResult Bad()
        {
            return BadRequest();
        }
    }
}
