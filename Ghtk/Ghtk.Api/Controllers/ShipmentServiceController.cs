using Ghtk.Api.Models;
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
        public async Task<IActionResult> CreateOrder([FromBody] OrderShipmentInput orderShipment)
        {
            return Ok();
        }   
    }
}
