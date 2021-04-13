using System;
using System.Collections.Generic;

namespace ReadingIsGood.Domain.Customer.Response
{
    public class OrdersResponse
    {
        public List<Order.Order> Orders { get; set; }
    }
}