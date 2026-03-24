using Client.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Client.Controllers
{
    public class ApiController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<ApiController> _logger;
        public ApiController(UserManager<IdentityUser> userManager, IHttpClientFactory httpClientFactory, ILogger<ApiController> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this._logger = logger;
        }

        [Route("/api/weatherforcast/v1/free")]
        [HttpGet]
        public async Task<IActionResult> GetFreeWeatherForcastAsync()
        {
            var httpClient = httpClientFactory.CreateClient();

            var response = await httpClient.GetAsync("https://localhost:7031/weatherforcast/free");

            var accessTokenClaim = User.Claims.Where(c => c.Type == "access_token").FirstOrDefault();
            if (accessTokenClaim != null)
            {
                _logger.LogInformation($"GetFreeWeatherForcastAsync - access token: {accessTokenClaim.Value}");
            }

            if (response != null && response.IsSuccessStatusCode)
            {
                return Json(JsonSerializer.Deserialize<WeatherForecast[]>(await response.Content.ReadAsStringAsync()));
            }
            else
            {
                return Content(response?.ReasonPhrase ?? "Error");
            }
        }

        [Route("/api/weatherforcast/v1/detailed")]
        [HttpGet]
        public async Task<IActionResult> GetDetailedWeatherForcastAsync()
        {
            var httpClient = httpClientFactory.CreateClient();
            // send access_token 
            var accessTokenClaim = User.Claims.Where(c => c.Type == "access_token").FirstOrDefault();
            if (accessTokenClaim != null)
            {
                _logger.LogInformation($"GetDetailedWeatherForcastAsync - access token: {accessTokenClaim.Value}");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessTokenClaim.Value);
            }

            var response = await httpClient.GetAsync("https://localhost:7031/weatherforcast/detailed");
            if (response != null && response.IsSuccessStatusCode)
            {
                return Json(JsonSerializer.Deserialize<WeatherForecast[]>(await response.Content.ReadAsStringAsync()));
            }
            else
            {
                return Content(response?.ReasonPhrase ?? "Error");
            }
        }
    }
}