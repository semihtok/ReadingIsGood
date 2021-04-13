using System;
using System.Collections.Generic;
using System.Linq;
using ReadingIsGood.Domain.Product;
using ReadingIsGood.Domain.Product.Request;
using ReadingIsGood.Domain.Product.Response;
using ReadingIsGood.Infrastructure.Context;

namespace ReadingIsGood.Infrastructure.Services.Implementations
{
    public class ProductService : IProductService
    {
        public int Create(ProductRequest productRequest)
        {
            try
            {
                using (var db = new ReadingIsGoodDbContext())
                {
                    var product = new Product
                    {
                        Name = productRequest.Name,
                        Description = productRequest.Description,
                        Quantity = productRequest.Quantity,
                        CreatedAt = DateTime.Now
                    };

                    db.Products.Add(product);
                    db.SaveChanges();

                    return product.ProductId;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 0;
            }
        }

        public bool Delete(ProductDeleteRequest productDeleteRequest)
        {
            try
            {
                using (var db = new ReadingIsGoodDbContext())
                {
                    var product = db.Products.FirstOrDefault(i => i.ProductId == productDeleteRequest.Id);
                    if (product != null)
                    {
                        db.Products.Remove(product);
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public List<Product> List()
        {
            try
            {
                using (var db = new ReadingIsGoodDbContext())
                {
                    var products = db.Products.ToList();
                    if (products.Any())
                    {
                        return products;
                    }
                    return new List<Product>();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<Product>();
            }
        }
    }
}