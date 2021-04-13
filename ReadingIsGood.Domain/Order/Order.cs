using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReadingIsGood.Domain.Order
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }
        public Customer.Customer Customer { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Order()
        {
            OrderItems = new List<OrderItem>();
        }
    }
}