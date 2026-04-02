using Ghtk.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ghtk.Api.Controllers
{
    [ApiController]
    [Route("/services/shipment")]
    public class ShipmentServiceController : ControllerBase
    {
        public ShipmentServiceController()
        {
            
        }


        [HttpPost]
        [Route("order")]
        [Authorize]
        public async Task<IActionResult> CreateOrder([FromBody] OrderShipmentInput orderShipment)
        {
            return Ok();
        }   
    }
}
