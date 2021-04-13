using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ReadingIsGood.Domain;
using ReadingIsGood.Domain.Order.Request;
using ReadingIsGood.Domain.Order.Response;
using ReadingIsGood.Domain.Product.Request;
using ReadingIsGood.Tests.Integration;
using Xunit;

namespace ReadingIsGood.Tests.Integration
{
    [Collection("SingleRun")]
    public class OrderTests : IClassFixture<TestFixture>
    {
        private readonly HttpClient _client;

        public OrderTests(TestFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task Can_Customer_Create_Order()
        {
            var orderRequest = new OrderRequest
            {
                Products = new List<OrderProductRequest>
                {
                    new() {Id = 1, Quantity = 1}
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/order/placement") {Content = new StringContent(JsonConvert.SerializeObject(orderRequest), Encoding.UTF8, "application/json")};
            request.Headers.Add("Authorization", $"Bearer {CustomerTests.Token}");
            var response = await _client.SendAsync(request);

            var responseData = response.Content.ReadAsStringAsync().Result;
            var orderResponse = JsonConvert.DeserializeObject<BaseResponse<OrderResponse>>(responseData);

            if (response.IsSuccessStatusCode)
            {
                Assert.Empty(orderResponse.Errors);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
            else
            {
                Assert.False(true);
            }
        }

        [Fact]
        public async Task Can_Customer_Create_Order_Product_More()
        {
            var orderRequest = new OrderRequest
            {
                Products = new List<OrderProductRequest>
                {
                    new() {Id = 1, Quantity = 11}
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/order/placement") {Content = new StringContent(JsonConvert.SerializeObject(orderRequest), Encoding.UTF8, "application/json")};
            request.Headers.Add("Authorization", $"Bearer {CustomerTests.Token}");
            var response = await _client.SendAsync(request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}