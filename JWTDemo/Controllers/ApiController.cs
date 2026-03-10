using System.Security.Claims;

namespace JWTDemo.Controllers
{
    [Authorize]
    public class ApiController : Controller
    {
        [Authorize(Policy = "FinanceOnly")]
        public IActionResult Hello5(string name)
        {
            return Content($"Hello {name}");
        }

        [Authorize(Roles = "Role1")]
        [Authorize(Roles = "Role2")]
        public IActionResult Hello1(string name)
        {
            return Content($"Hello {name}");
        }

        [Authorize(Roles = "Role1, Role2")]
        public IActionResult Hello2(string name)
        {
            return Content($"Hello {name}");
        }

        [Authorize(Roles = "Role3")]
        public IActionResult Hello3(string name)
        {
            return Content($"Hello {name}");
        }

        [Authorize(Policy = "PolicyBoth12AndSomeClaims")]
        public IActionResult Hello4(string name)
        {
            return Content($"Hello {name}");
        }

        [AllowAnonymous]
        public IActionResult Hello()
        {
            return Content($"Hello Anonymous");
        }
    }
}
