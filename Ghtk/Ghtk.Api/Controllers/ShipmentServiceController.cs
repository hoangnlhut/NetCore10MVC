using Ghtk.Api.Models;
using GhtkRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ghtk.Api.Controllers
{
    [ApiController]
    [Route("/services/shipment")]
    [Authorize]
    public class ShipmentServiceController : ControllerBase
    {
        private readonly ILogger<ShipmentServiceController> _logger;
        private readonly IOrderRepository _orderRepository;
        public ShipmentServiceController(IOrderRepository orderRepository, ILogger<ShipmentServiceController> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        [HttpPost]
        [Route("order")]
        public async Task<IActionResult> OrderCreatetion([FromBody] OrderCreationRequestModel orderCreateionInput)
        {
            if (orderCreateionInput == null)
            {
                return BadRequest("Invalid order creation request.");
            }

            var partnerId = User.FindFirst("PartnerId")!.Value;
            if (string.IsNullOrEmpty(partnerId))
            {
                return Unauthorized("PartnerId claim is missing.");
            }

            var order = new Order()
            {
                Id = orderCreateionInput.Order.Id,
                TrackingId = Guid.NewGuid().ToString(),
                PartnerId = partnerId,
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow,
                PickName = orderCreateionInput.Order.PickName,
                PickAddress = orderCreateionInput.Order.PickAddress,
                PickProvince = orderCreateionInput.Order.PickProvince,
                PickDistrict = orderCreateionInput.Order.PickDistrict,
                PickWard = orderCreateionInput.Order.PickWard,
                PickTel = orderCreateionInput.Order.PickTel,
                Tel = orderCreateionInput.Order.Tel,
                Name = orderCreateionInput.Order.Name,
                Address = orderCreateionInput.Order.Address,
                Province = orderCreateionInput.Order.Province,
                District = orderCreateionInput.Order.District,
                Ward = orderCreateionInput.Order.Ward,
                Hamlet = orderCreateionInput.Order.Hamlet,
                Value = orderCreateionInput.Order.Value,
                PickMoney = orderCreateionInput.Order.PickMoney,
                Note = orderCreateionInput.Order.Note,
                Transport = orderCreateionInput.Order.Transport,
                PickOption = orderCreateionInput.Order.PickOption,
                Status = 1,
                GamSolutions = orderCreateionInput.Order.GamSolutions.Select(x => new GhtkRepository.GamSolution()
                {
                    SolutionId = x.SolutionId
                }).ToList(),
                Products = orderCreateionInput.Products.Select(x => new GhtkRepository.Product()
                {
                    Name = x.Name,
                    Weight = x.Weight,
                    Quantity = x.Quantity,
                    ProductCode = x.ProductCode
                }).ToList()

            };

            await _orderRepository.CreateOrderAsync(order);

            var result = new OrderCreationResponseModel()
            {
                Success = true,
                Message = string.Empty,
                Order = new OrderCreationResponseOrder()
                {
                    PartnerId = order.PartnerId,
                    Label = "S1.A1.2001297581",
                    Area = "1",
                    Fee = order.Value.ToString(),
                    InsuranceFee = "15000",
                    TrackingId = order.TrackingId.ToString(),
                    EstimatedPickTime = order.PickDate.ToString(),
                    EstimatedDeliverTime = order.DeliverDate.ToString(),
                    Products = order.Products.Select(x => new Models.Product()
                    {
                        Name = x.Name,
                        Weight = x.Weight,
                        ProductCode = x.ProductCode,
                        Quantity = x.Quantity,
                    }).ToList(),
                    StatusId = 2
                }
            };

            return Ok(result);
        }

        [HttpGet]
        [Route("v2/{trackingId}")]
        public async Task<IActionResult> RetrieveOrderStatus(string trackingId)
        {
            if (trackingId == null)
            {
                return BadRequest();
            }

            var partnerId = User.FindFirst("PartnerId")!.Value;
            if (string.IsNullOrEmpty(partnerId))
            {
                return Unauthorized("PartnerId claim is missing.");
            }

            var order = await _orderRepository.GetOrderAsync(trackingId, partnerId);
            if (order == null)
            {
                return NotFound();
            }

            var orderResponseModel = new OrderStatusResponseModel()
            {
                Success = true,
                Message = "",
                Order = new OrderStatusResponseOrder()
                {
                    LabelId = order.Id,
                    PartnerId = order.PartnerId,
                    Status = order.Status.ToString(),
                    StatusText = "Chưa tiếp nhận",
                    Created = order.Created.ToString(),
                    Modified = order.Modified.ToString(),
                    Message = order.Message,
                    PickDate = order.PickDate.ToString(),
                    DeliverDate = order.DeliverDate.ToString(),
                    CustomerFullname = order.Name,
                    CustomerTel = order.Tel,
                    Address = order.Address,
                    StorageDay = "3",
                    ShipMoney = order.PickMoney.ToString(),
                    Insurance = order.PickMoney.ToString(),
                    Value = order.Value.ToString(),
                    Weight = "300",
                    PickMoney = order.PickMoney,
                    IsFreeship = "1"
                }
            };


            return Ok(orderResponseModel);
        }

        [HttpGet]
        [Route("cancel/{trackingId}")]
        public async Task<IActionResult> CancelOrder(string trackingId)
        {
            if (trackingId == null)
            {
                return BadRequest();
            }

            var partnerId = User.FindFirst("PartnerId")!.Value;
            if (string.IsNullOrEmpty(partnerId))
            {
                return Unauthorized("PartnerId claim is missing.");
            }

            var resuls =  await _orderRepository.CancelOrderAsync(trackingId, partnerId);
            
            if(!resuls)
            {
                return NotFound();
            }

            var cancelOrderResponseModel = new CancerOrderResponseModel()
            {
                Success = true,
                Message = "",
                LogId = "...."
            };
            return Ok(cancelOrderResponseModel);
        }


        //[HttpGet]
        //[Route("v3/{hoang}")]
        //public IActionResult RetrieveOrderStatus([FromRoute] string hoang, [FromQuery] string? trang)
        //{
        //    return Ok();
        //}
    }
}