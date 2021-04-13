using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadingIsGood.API.Helpers;
using ReadingIsGood.Domain;
using ReadingIsGood.Domain.Customer.Response;
using ReadingIsGood.Domain.Order.Request;
using ReadingIsGood.Domain.Order.Response;
using ReadingIsGood.Infrastructure.Services;

namespace ReadingIsGood.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IValidationHelper _validationHelper;

        public OrderController(IOrderService orderService, IValidationHelper validationHelper)
        {
            _orderService = orderService;
            _validationHelper = validationHelper;
        }

        [HttpPost]
        [Authorize]
        [Route("Placement")]
        [Consumes("application/json")]
        public async Task<IActionResult> Create(OrderRequest orderRequest)
        {
            var response = new BaseResponse<OrderResponse>();

            if (!ModelState.IsValid)
            {
                var validationErrors = _validationHelper.GetValidationErrors(ModelState);
                response.Errors = validationErrors;
                return BadRequest(response);
            }

            if (User.Identity != null)
            {
                var result = await _orderService.Placement(orderRequest.Products, User.Identity.Name);
                if (result)
                {
                    response.Message = "Order created successfully";
                    return Ok(response);
                }
            }

            return BadRequest();
        }

        [HttpGet]
        [Authorize]
        [Route("Detail")]
        [Consumes("application/json")]
        public async Task<IActionResult> Detail([FromQuery] Guid orderId)
        {
            var response = new BaseResponse<OrderDetailsResponse>();

            if (!ModelState.IsValid)
            {
                var validationErrors = _validationHelper.GetValidationErrors(ModelState);
                response.Errors = validationErrors;
                return BadRequest(response);
            }

            if (User.Identity != null)
            {
                var result = await _orderService.OrderDetailsByCustomer(orderId, User.Identity.Name);
                if (result != null)
                {
                    response.Data = new OrderDetailsResponse
                    {
                        OrderItems = result.OrderItems,
                        CreatedAt = result.CreatedAt,
                        UpdatedAt = result.UpdatedAt
                    };
                    response.Total = 1;
                    return Ok(response);
                }
            }
        
            return BadRequest();
        }

        [HttpDelete]
        [Authorize]
        [Route("Delete")]
        [Consumes("application/json")]
        public async Task<IActionResult> Delete([FromBody] OrderDeleteRequest orderDeleteRequest)
        {
            var response = new BaseResponse<string>();

            if (!ModelState.IsValid)
            {
                var validationErrors = _validationHelper.GetValidationErrors(ModelState);
                response.Errors = validationErrors;
                return BadRequest(response);
            }

            if (User.Identity != null)
            {
                var result = await _orderService.Delete(orderDeleteRequest.Id);
                if (result)
                {
                    response.Message = "Order created successfully";
                    return Ok(response);
                }
            }

            return BadRequest();
        }
    }
}