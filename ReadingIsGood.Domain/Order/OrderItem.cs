using System;
using System.Text.Json.Serialization;
using ReadingIsGood.Domain.Entity;

namespace ReadingIsGood.Domain.Order
{
    public class OrderItem : EntityBase
    {
        [JsonIgnore]
        public Order Order { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Product.Product Product { get; set; }
    }
}