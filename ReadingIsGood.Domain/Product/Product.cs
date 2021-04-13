using System;
using ReadingIsGood.Domain.Order;

namespace ReadingIsGood.Domain.Product
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderItem OrderItem { get; set; }
    }
}