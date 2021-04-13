using System.ComponentModel.DataAnnotations;
using ReadingIsGood.Domain.Entity;

namespace ReadingIsGood.Domain.Product.Request
{
    public class OrderProductRequest : EntityBase
    {
        [Required(ErrorMessage = "Quantity is required")]
        public int Quantity { get; set; }
    }
}