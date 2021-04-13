using System;
using ReadingIsGood.Domain.Customer;
using ReadingIsGood.Domain.Product;

namespace ReadingIsGood.Infrastructure.Context
{
    public class SeedData
    {
        public void Seed()
        {
            using (var db = new ReadingIsGoodDbContext())
            {
                var productXbox = new Product
                {
                    Name = "Xbox Series X",
                    Description = "Powerful gaming console",
                    Price = 7000,
                    CreatedAt = DateTime.Now
                };

                var productPs5 = new Product
                {
                    Name = "Playstation 5",
                    Description = "Classic gaming console",
                    Price = 8000,
                    CreatedAt = DateTime.Now
                };

                db.Products.Add(productXbox);
                db.Products.Add(productPs5);

                var testCustomer = new Customer
                {
                    Email = "test@test.com",
                    UserName = "test_user",
                    PasswordHash = "AQAAAAEAACcQAAAAEExdmtPsPrYHO327y5yYNOutbWgJHQYh5M8Eq6bKAOrwCwS4+PCOU36IN8IhIpnNPA==",
                    CreatedAt = DateTime.Now
                };

                db.Customers.Add(testCustomer);
                
                db.SaveChanges();
            }
        }
    }
}