using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReadingIsGood.Domain.Order;
using ReadingIsGood.Domain.Product.Request;
using ReadingIsGood.Infrastructure.Context;

namespace ReadingIsGood.Infrastructure.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private static readonly SemaphoreSlim OrderLock = new(1);
        
        /// <summary>
        /// Placement adds new order record with user data. It uses thread lock for safe data processing.
        /// </summary>
        /// <param name="products"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<bool> Placement(List<OrderProductRequest> products, string customerId)
        {
            try
            {
                await OrderLock.WaitAsync();
                await using (var db = new ReadingIsGoodDbContext())
                {
                    var customer = db.Customers.FirstOrDefault(i => i.Id == customerId);

                    var order = new Order
                    {
                        Customer = customer,
                        OrderItems = new List<OrderItem>(),
                        CreatedAt = DateTime.Now
                    };

                    var productIds = products.Select(i => i.Id);
                    var foundProducts = db.Products.Where(i => productIds.Contains(i.ProductId)).ToList();

                    foreach (var product in products)
                    {
                        var currentProduct = foundProducts.FirstOrDefault(i => i.ProductId == product.Id);
                        if (currentProduct is {Quantity: > 0} && currentProduct.Quantity > product.Quantity)
                        {
                            currentProduct.Quantity -= product.Quantity;
                            order.OrderItems.Add(new OrderItem
                            {
                                Quantity = product.Quantity,
                                Order = order,
                                Product = currentProduct,
                                CreatedAt = DateTime.Now
                            });
                        }
                    }

                    if (order.OrderItems.Any())
                    {
                        db.Orders.Add(order);
                        await db.SaveChangesAsync();
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
            finally
            {
                OrderLock.Release();
            }
        }

        public async Task<List<Order>> OrdersByCustomer(string customerId)
        {
            try
            {
                await using (var db = new ReadingIsGoodDbContext())
                {
                    var result = db.Orders.Where(i => i.Customer.Id == customerId).Include("OrderItems").ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        
        public async Task<Order> OrderDetailsByCustomer(Guid orderId,string customerId)
        {
            try
            {
                await using (var db = new ReadingIsGoodDbContext())
                {
                    var result = db.Orders.Include("OrderItems").FirstOrDefault(i => i.Customer.Id == customerId && i.Id == orderId);
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<bool> Delete(Guid orderId)
        {
            try
            {
                using (var db = new ReadingIsGoodDbContext())
                {
                    var order = db.Orders.FirstOrDefault(i => i.Id == orderId);
                    if (order != null)
                    {
                        db.Orders.Remove(order);
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
    }
}