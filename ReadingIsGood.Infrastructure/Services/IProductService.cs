using System.Collections.Generic;
using ReadingIsGood.Domain.Product;
using ReadingIsGood.Domain.Product.Request;
using ReadingIsGood.Domain.Product.Response;

namespace ReadingIsGood.Infrastructure.Services
{
    public interface IProductService
    {
        int Create(ProductRequest productRequest);
        bool Delete(ProductDeleteRequest productDeleteRequest);
        List<Product> List();
    }
}