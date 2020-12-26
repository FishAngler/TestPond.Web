using TestPond.BusinessLayer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;

namespace TestPond.Web.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public LoginModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
        public string LoginUserName { get; set; }

        [BindProperty, DataType(DataType.Password)]
        public string LoginPassword { get; set; }

        public string Message { get; set; }

        public async Task<IActionResult> OnPost()
        {
            var registeredUsers = _configuration.GetSection("UserRoles").Get<List<UserRole>>();

            //TODO: Handle "User not found"
            var user = registeredUsers.Where(x => x.UserName == LoginUserName).Single();

            var passwordHasher = new PasswordHasher<string>();

            //TODO: Handle "Password does not match"
            if (passwordHasher.VerifyHashedPassword(null, user.HashedPassword, LoginPassword) == PasswordVerificationResult.Success)
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, LoginUserName)
                    };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                //Admin Index Page is not displaying properly afte the user "should be signed in"...
                return RedirectToPage("/Index");
            }

            Message = "Invalid attempt";

            return Page();
        }
    }
}