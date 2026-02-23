using Microsoft.AspNetCore.Mvc;
using MiddlewareDemo.ClientInfoRepository;
using MiddlewareDemo.Models;

namespace MiddlewareDemo.Controllers
{
    public class ClientInfoController : Controller
    {
        private readonly IClientInfoRepository _repository;

        public ClientInfoController(IClientInfoRepository repository)
        {
            _repository = repository;
        }

        [Route("/clientinfo")]
        [HttpGet]
        public IActionResult? GetClientInfo([FromHeader(Name = "Api-Key")] string apiKey)
        {
            //using this way if name of header has "-"  or if you want to use different name from header name in method parameter like this [FromHeader(Name = "Api-Key")] string apiKey
            //string? apiKeyLast = Request.Headers["Api-Key"];
            //apiKeyLast = string.IsNullOrEmpty(apiKeyLast) ? apiKey : apiKeyLast;

            if (string.IsNullOrEmpty(apiKey))
            {
                BadRequest();
            }
            var clientInfo = _repository.GetClientInfo(apiKey);
            
            return clientInfo != null ? Ok(clientInfo) : Unauthorized();
        }
    }
}
