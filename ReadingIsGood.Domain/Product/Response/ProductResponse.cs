using ReadingIsGood.Domain.Customer.Response;

namespace ReadingIsGood.Domain.Product.Response
{
    public class ProductResponse
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}