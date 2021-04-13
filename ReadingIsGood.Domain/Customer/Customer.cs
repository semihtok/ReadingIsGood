using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ReadingIsGood.Domain.Customer
{
    public class Customer : IdentityUser
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public ICollection<Order.Order> Orders { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}