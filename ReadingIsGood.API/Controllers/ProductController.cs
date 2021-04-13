using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReadingIsGood.API.Helpers;
using ReadingIsGood.Domain;
using ReadingIsGood.Domain.Product;
using ReadingIsGood.Domain.Product.Request;
using ReadingIsGood.Domain.Product.Response;
using ReadingIsGood.Infrastructure.Services;

namespace ReadingIsGood.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IValidationHelper _validationHelper;

        public ProductController(IProductService productService, IValidationHelper validationHelper)
        {
            _productService = productService;
            _validationHelper = validationHelper;
        }

        [HttpPost]
        [Route("Create")]
        [Consumes("application/json")]
        public async Task<IActionResult> Create([FromBody] ProductRequest productRequest)
        {
            var response = new BaseResponse<ProductResponse> {Data = new ProductResponse()};

            if (!ModelState.IsValid)
            {
                var validationErrors = _validationHelper.GetValidationErrors(ModelState);
                response.Errors = validationErrors;
                return BadRequest(response);
            }

            var result = _productService.Create(productRequest);
            if (result != 0)
            {
                response.Data.ProductId = result;
                response.Message = "Product created successfully";
                return Ok(response);
            }

            response.Message = "Product could't be created";
            return BadRequest(response);
        }
        
        [HttpGet]
        [Route("List")]
        [Consumes("application/json")]
        public async Task<IActionResult> List()
        {
            var response = new BaseResponse<List<Product>> {Data = new List<Product>()};

            var result = _productService.List();
            if (result.Any())
            {
                response.Data = result;
                response.Message = "Products listed successfully";
                return Ok(response);
            }

            response.Message = "Products could't be listed";
            return BadRequest(response);
        }
        

        [HttpDelete]
        [Route("Delete")]
        [Consumes("application/json")]
        public async Task<IActionResult> Delete(ProductDeleteRequest productDeleteRequest)
        {
            var response = new BaseResponse<string>();

            if (!ModelState.IsValid)
            {
                var validationErrors = _validationHelper.GetValidationErrors(ModelState);
                response.Errors = validationErrors;
                return BadRequest(response);
            }

            var result = _productService.Delete(productDeleteRequest);
            if (result)
            {
                response.Message = "Product deleted successfully";
                return Ok(response);
            }

            response.Message = "Product could't be deleted";
            return BadRequest(response);
        }
    }
}