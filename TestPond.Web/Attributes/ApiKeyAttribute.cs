using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TestPond.Web.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Class)]
    public class ApiKeyRequiredAttribute : Attribute, IAsyncActionFilter
    {
        private const string APIKEYNAME = "API_Key";
        private const string APIKEYHASHNAME = "API_Key_Hash";

        public ApiKeyRequiredAttribute()
        {
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(APIKEYNAME, out var clientProvidedAPIKey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content =$"{APIKEYNAME} was not provided"
                };
                return;
            }

            var appSettings = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var storedAPIKeyHash = appSettings.GetValue<string>(APIKEYHASHNAME);

            var passwordHasher = new PasswordHasher<string>();

            if (passwordHasher.VerifyHashedPassword(null, storedAPIKeyHash, clientProvidedAPIKey) != PasswordVerificationResult.Success)
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = $"{APIKEYHASHNAME} is not valid"
                };
                return;
            }

            await next();
        }
    }
}
