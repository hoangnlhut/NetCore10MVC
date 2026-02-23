using Microsoft.AspNetCore.Mvc;

namespace MiddlewareDemo.Controllers
{
    public class ClientInfoMiddlewareController : Controller
    {
        private readonly IClientInfoRepository _repository;

        public ClientInfoMiddlewareController(IClientInfoRepository repository)
        {
            _repository = repository;
        }

        [Route("/clientinfomiddleware")]
        [HttpGet]
        public IActionResult GetClientInfo()
        {
            return Ok(this.HttpContext.Features.Get<ClientInfo>());
        }
    }
}
