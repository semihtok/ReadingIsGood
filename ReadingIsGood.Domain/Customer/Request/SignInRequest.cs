using System.ComponentModel.DataAnnotations;

namespace ReadingIsGood.Domain.Customer.Request
{
    public class SignInRequest
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}