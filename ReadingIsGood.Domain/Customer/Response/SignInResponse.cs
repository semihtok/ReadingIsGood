using System;

namespace ReadingIsGood.Domain.Customer.Response
{
    public class SignInResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}