using System;
using System.Collections.Generic;
using ReadingIsGood.Domain.Customer.Response;

namespace ReadingIsGood.Domain.Order.Response
{
    public class OrderDetailsResponse
    {
        public ICollection<OrderItem> OrderItems { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public OrderDetailsResponse()
        {
            OrderItems = new List<OrderItem>();
        }
    }
}