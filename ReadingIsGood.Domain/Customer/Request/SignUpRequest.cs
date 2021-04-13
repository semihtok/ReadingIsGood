using System.ComponentModel.DataAnnotations;

namespace ReadingIsGood.Domain.Customer.Request
{
    public class SignUpRequest
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}