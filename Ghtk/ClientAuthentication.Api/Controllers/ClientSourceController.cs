using Microsoft.AspNetCore.Mvc;

namespace ClientAuthentication.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientSourceController : ControllerBase
    {
        private readonly IClientSourceAuthenticationHandler _clientSourceAuthenticationHandler;

        public ClientSourceController(IClientSourceAuthenticationHandler clientSourceAuthenticationHandler) {
            _clientSourceAuthenticationHandler = clientSourceAuthenticationHandler;
        }

        [HttpGet("{id}")]
        public IActionResult Validate(string id)
        {
            var validate = _clientSourceAuthenticationHandler.Validate(id);
            if (!validate)
            {
                return Unauthorized("Invalid client source.");
            }
            
            return Ok("Client Source Authentication API is running.");
        }
    }
}
