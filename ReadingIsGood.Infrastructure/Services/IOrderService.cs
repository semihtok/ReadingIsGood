using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReadingIsGood.Domain.Order;
using ReadingIsGood.Domain.Product.Request;

namespace ReadingIsGood.Infrastructure.Services
{
    public interface IOrderService
    {
        Task<bool> Placement(List<OrderProductRequest> products, string customerId);
        Task<List<Order>> OrdersByCustomer(string customerId);
        Task<Order> OrderDetailsByCustomer(Guid orderId, string customerId);
        Task<bool> Delete(Guid orderId);
    }
}