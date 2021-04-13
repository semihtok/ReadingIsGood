using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ReadingIsGood.API.Helpers;
using ReadingIsGood.Domain;
using ReadingIsGood.Domain.Customer;
using ReadingIsGood.Domain.Customer.Request;
using ReadingIsGood.Domain.Customer.Response;
using ReadingIsGood.Domain.Order;
using ReadingIsGood.Infrastructure.Services;

namespace ReadingIsGood.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly UserManager<Customer> _userManager;
        private readonly ICustomerService _customerService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IOrderService _orderService;
        private readonly IValidationHelper _validationHelper;

        public CustomerController(UserManager<Customer> userManager, IConfiguration configuration,
            RoleManager<IdentityRole> roleManager, ICustomerService customerService, IOrderService orderService, IValidationHelper validationHelper)
        {
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _customerService = customerService;
            _orderService = orderService;
            _validationHelper = validationHelper;
        }

        [HttpPost]
        [Route("SignUp")]
        [Consumes("application/json")]
        public async Task<IActionResult> SignUp(SignUpRequest signUpRequest)
        {
            var response = new BaseResponse<SignInResponse>();

            if (!ModelState.IsValid)
            {
                var validationErrors = _validationHelper.GetValidationErrors(ModelState);
                response.Errors = validationErrors;
                return BadRequest(response);
            }

            var errors = await _customerService.Create(signUpRequest, _userManager, _roleManager);
            if (errors == null)
            {
                response.Message = "Customer created successfully";
                return Ok(response);
            }

            foreach (var identityError in errors)
            {
                response.Errors.Add(identityError.Description);
            }

            response.Message = "Customer couldn't be registered";
            return BadRequest(response);
        }

        [HttpPost]
        [Route("SignIn")]
        [Consumes("application/json")]
        public async Task<IActionResult> SignIn(SignInRequest signInRequest)
        {
            var response = new BaseResponse<SignInResponse>();

            if (!ModelState.IsValid)
            {
                var validationErrors = _validationHelper.GetValidationErrors(ModelState);
                response.Errors = validationErrors;
                return BadRequest(response);
            }

            var user = await _userManager.FindByEmailAsync(signInRequest.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, signInRequest.Password))
            {
                var token = _customerService.SignIn(user, _userManager, _configuration);

                if (token != null)
                {
                    var tokenResult = new JwtSecurityTokenHandler().WriteToken(token);
                    response.Data = new SignInResponse
                    {
                        Token = tokenResult,
                        Expiration = token.ValidTo
                    };
                    response.Total = 1;
                    response.Message = "Customer signed in successfully";

                    return Ok(response);
                }
            }

            response.Errors.Add("Customer couldn't be signed in");
            return Unauthorized(response);
        }

        [HttpGet]
        [Route("Informations")]
        [Authorize]
        [Consumes("application/json")]
        public async Task<IActionResult> Informations()
        {
            var response = new BaseResponse<InformationResponse>();
            if (!ModelState.IsValid)
            {
                var validationErrors = _validationHelper.GetValidationErrors(ModelState);
                response.Errors = validationErrors;
                return BadRequest(response);
            }

            if (User.Identity != null)
            {
                var user = await _userManager.FindByIdAsync(User.Identity.Name);

                if (user != null)
                {
                    response.Data = new InformationResponse
                    {
                        Username = user.UserName,
                        CreatedAt = user.CreatedAt
                    };
                    response.Total = 1;
                    response.Message = "Customer informations found successfully";
                    return Ok(response);
                }

                response.Message = "Customer informations couldn't be found";
                return NotFound(response);
            }

            return Unauthorized();
        }

        [HttpGet]
        [Route("Orders")]
        [Authorize]
        [Consumes("application/json")]
        public async Task<IActionResult> Orders()
        {
            var response = new BaseResponse<OrdersResponse> {Data = new OrdersResponse {Orders = new List<Order>()}};
            if (!ModelState.IsValid)
            {
                var validationErrors = _validationHelper.GetValidationErrors(ModelState);
                response.Errors = validationErrors;
                return BadRequest(response);
            }

            if (User.Identity != null)
            {
                var customerOrders = await _orderService.OrdersByCustomer(User.Identity.Name);
                if (customerOrders.Any())
                {
                    response.Data.Orders = customerOrders;
                    response.Total = customerOrders.Count;
                    return Ok(response);
                }

                response.Message = "Customer order informations couldn't be found";
                return NotFound(response);
            }
            response.Message = "User not authorized";
            return Unauthorized(response);
        }

        [HttpDelete]
        [Route("Delete")]
        [Consumes("application/json")]
        public async Task<IActionResult> Delete([FromBody] DeleteRequest signInRequest)
        {
            var response = new BaseResponse<DeleteResponse>();
            if (!ModelState.IsValid)
            {
                var validationErrors = _validationHelper.GetValidationErrors(ModelState);
                response.Errors = validationErrors;
                return BadRequest(response);
            }

            var user = await _userManager.FindByEmailAsync(signInRequest.Email);
            if (user != null)
            {
                var result = _customerService.Delete(user, _userManager).Result;

                if (result.Succeeded)
                {
                    response.Message = "Customer deleted successfully";
                    return Ok(response);
                }
            }

            response.Message = "Customer couldn't be deleted";
            return BadRequest(response);
        }
    }
}