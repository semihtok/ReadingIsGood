using System.ComponentModel.DataAnnotations;

namespace ReadingIsGood.Domain.Customer.Request
{
    public class DeleteRequest
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }
    }
}