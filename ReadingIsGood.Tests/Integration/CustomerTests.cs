using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ReadingIsGood.Domain;
using ReadingIsGood.Domain.Customer.Request;
using ReadingIsGood.Domain.Customer.Response;
using Xunit;

namespace ReadingIsGood.Tests.Integration
{
    [Collection("SingleRun")]
    public class CustomerTests : IClassFixture<TestFixture>
    {
        private readonly HttpClient _client;

        // Test user informations
        private const string Email = "test@test.com";
        private const string Password = "Qwerty123.";
        public static string Token = "";

        public CustomerTests(TestFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task Can_Customer_SignIn()
        {
            var signInRequest = new SignInRequest
            {
                Email = Email,
                Password = Password
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/customer/signin") {Content = new StringContent(JsonConvert.SerializeObject(signInRequest), Encoding.UTF8, "application/json")};
            var response = await _client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseData = response.Content.ReadAsStringAsync().Result;
                var signInResponse = JsonConvert.DeserializeObject<BaseResponse<SignInResponse>>(responseData);
                Token = signInResponse.Data.Token;

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(!string.IsNullOrEmpty(Token));
            }
            else
            {
                Assert.False(true);
            }
        }

        [Fact]
        public async Task Can_Customer_Signup()
        {
            var random = new Random();
            var id = random.Next(0, int.MaxValue);

            var signUpRequest = new SignUpRequest
            {
                Email = $"test{id}@test.com",
                Username = $"test_user{id}",
                Password = "Qwerty123."
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/customer/signup") {Content = new StringContent(JsonConvert.SerializeObject(signUpRequest), Encoding.UTF8, "application/json")};
            var response = await _client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
               Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
            else
            {
                Assert.False(true);
            }
        }
    }
}