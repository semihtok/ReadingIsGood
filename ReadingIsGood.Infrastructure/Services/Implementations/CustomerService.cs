using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ReadingIsGood.Domain.Customer;
using ReadingIsGood.Domain.Customer.Request;
using ReadingIsGood.Infrastructure.Context;

namespace ReadingIsGood.Infrastructure.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        public async Task<Customer> Get(SignInRequest request)
        {
            await using (var db = new ReadingIsGoodDbContext())
            {
                var foundUser = db.Customers.FirstOrDefault(u => u.Email == request.Email);
                return foundUser ?? new Customer();
            }
        }

        /// <summary>
        /// Create adds new user record to database
        /// </summary>
        /// <param name="signUpRequest"></param>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <returns></returns>
        public async Task<IEnumerable<IdentityError>> Create(SignUpRequest signUpRequest, UserManager<Customer> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            try
            {
                var result = await userManager.CreateAsync(new Customer
                {
                    Email = signUpRequest.Email,
                    UserName = signUpRequest.Username,
                    CreatedAt = DateTime.Now
                }, signUpRequest.Password);

                if (result.Succeeded)
                {
                    var user = userManager.FindByEmailAsync(signUpRequest.Email).Result;

                    if (user != null)
                    {
                        if (!await roleManager.RoleExistsAsync(UserRoles.Customer))
                        {
                            await roleManager.CreateAsync(new IdentityRole {Name = UserRoles.Customer, NormalizedName = UserRoles.Customer.ToUpper()});
                        }
                        await userManager.AddToRoleAsync(user, UserRoles.Customer);
                    }
                    return null;
                }

                return result.Errors;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<IdentityError>();
            }
        }

        /// <summary>
        /// SignIn finds current user and process JWT authentication flow
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="userManager"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public JwtSecurityToken SignIn(Customer customer, UserManager<Customer> userManager, IConfiguration configuration)
        {
            var userRoles = userManager.GetRolesAsync(customer).Result;
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, customer.Id.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            
            claims.Add(new Claim(ClaimTypes.Sid,customer.Id.ToString()));

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: claims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
            return token;
        }
        
        public Task<IdentityResult> Delete(Customer customer, UserManager<Customer> userManager)
        {
            return userManager.DeleteAsync(customer);
        }
    }
}