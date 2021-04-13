using ReadingIsGood.Domain.Product.Request;

namespace ReadingIsGood.Infrastructure.Services
{
    public interface IProductService
    {
        int Create(ProductRequest productRequest);
        bool Delete(ProductDeleteRequest productDeleteRequest);
    }
}