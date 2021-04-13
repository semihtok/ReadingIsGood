using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ReadingIsGood.Domain.Product.Request;

namespace ReadingIsGood.Domain.Order.Request
{
    public class OrderRequest
    {
        [Required(ErrorMessage = "Product(s) is required")]
        public List<OrderProductRequest> Products { get; set; }
    }
}