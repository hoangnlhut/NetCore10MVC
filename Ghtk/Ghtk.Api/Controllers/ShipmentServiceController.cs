using Ghtk.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ghtk.Api.Controllers
{
    [ApiController]
    [Route("/services/shipment")]
    [Authorize]
    public class ShipmentServiceController : ControllerBase
    {
        public ShipmentServiceController()
        {

        }

        [HttpPost]
        [Route("order")]
        public async Task<IActionResult> OrderCreatetion([FromBody] OrderCreationRequestModel orderCreateionInput)
        {

            var order = new OrderCreationResponseModel()
            {
                Success = true,
                Message = string.Empty,
                Order = new OrderCreationResponseOrder()
                {
                    PartnerId = "123123a",
                    Label = "S1.A1.2001297581",
                    Area = "1",
                    Fee = "30400",
                    InsuranceFee = "15000",
                    TrackingId = 2001297581,
                    EstimatedPickTime = "Sáng 2017-07-01",
                    EstimatedDeliverTime = "Chiều 2017-07-01",
                    Products = [],
                    StatusId = 2
                }
            };

            return Ok(order);
        }


        [HttpGet]
        [Route("v2/{trackingId}")]
        public IActionResult RetrieveOrderStatus(string trackingId)
        {
            var orderResponseModel = new OrderStatusResponseModel()
            {
                Success = true,
                Message = "",
                Order = new OrderStatusResponseOrder()
                {
                    LabelId = "S1.A1.17373471",
                    PartnerId = "1234567",
                    Status = "1",
                    StatusText = "Chưa tiếp nhận",
                    Created = "2016-10-31 22:32:08",
                    Modified = "2016-10-31 22:32:08",
                    Message = "Không giao hàng 1 phần",
                    PickDate = "2017-09-13",
                    DeliverDate = "2017-09-14",
                    CustomerFullname = "Vân Nguyễn",
                    CustomerTel = "0911222333",
                    Address = "123 nguyễn chí thanh Quận 1, TP Hồ Chí Minh",
                    StorageDay = "3",
                    ShipMoney = "16500",
                    Insurance = "16500",
                    Value = "3000000",
                    Weight = "300",
                    PickMoney = 47000,
                    IsFreeship = "1"
                }
            };


            return Ok(orderResponseModel);
        }

        [HttpGet]
        [Route("cancel/{trackingId}")]
        public IActionResult CancelOrder(string trackingId)
        {
            var cancerOrderResponseModel = new CancerOrderResponseModel()
            {
                Success = true,
                Message = "",
                LogId = "...."
            };
            return Ok(cancerOrderResponseModel);
        }
    }
}