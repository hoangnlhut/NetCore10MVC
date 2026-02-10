using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SessionFeature.Controllers
{
    public class TestController : Controller
    {
        public IActionResult TestGetSession()
        {
            var session = HttpContext.GetSession();
            session.SetString("Name", "1234567");

            session = HttpContext.GetSession();
            var value = session.GetString("Name");

            if (value == "1234567")
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> SetSessionValue(string key, string value)
        {
            var session = HttpContext.GetSession();
            session.SetString(key, value);
            await session.CommitAsync();

            return Ok();
        }

        public async Task<IActionResult> GetSessionValue(string key)
        {
            var session = HttpContext.GetSession();
            await session.LoadAsync();
            var result =  session.GetString(key);
            return Ok(result);
        }
    }
}
