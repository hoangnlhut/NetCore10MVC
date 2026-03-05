namespace JWTDemo.Controllers
{
    [Authorize]
    public class ApiController : Controller
    {
        public IActionResult Hello(string name)
        {
            return Content($"Hello {name}");
        }
    }
}
