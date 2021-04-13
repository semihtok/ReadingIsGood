using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ReadingIsGood.Tests.Healthcheck
{
    public class HealthcheckTest : IClassFixture<TestFixture>
    {
        private readonly HttpClient _client;

        public HealthcheckTest(TestFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task Check_Is_Application_Healthy()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/healthcheck");
            var response = await _client.SendAsync(request);
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Healthy", response.Content.ReadAsStringAsync().Result);
        }
    }
}