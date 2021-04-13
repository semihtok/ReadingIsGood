using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using ReadingIsGood.Domain.Customer;
using ReadingIsGood.Domain.Customer.Request;

namespace ReadingIsGood.Infrastructure.Services
{
    public interface ICustomerService
    {
        Task<Customer> Get(SignInRequest signInRequest);
        Task<IEnumerable<IdentityError>> Create(SignUpRequest signUpRequest, UserManager<Customer> userManager, RoleManager<IdentityRole> roleManager);
        JwtSecurityToken SignIn(Customer customer, UserManager<Customer> userManager, IConfiguration configuration);
        Task<IdentityResult> Delete(Customer customer, UserManager<Customer> userManager);
    }
}