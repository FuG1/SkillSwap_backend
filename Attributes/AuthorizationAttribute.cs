using SkillSwap.Interfaces.Options;
using SkillSwap.Models;
using SkillSwap.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace SkillSwap.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AuthorizationAttribute() : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var jwtService = context.HttpContext.RequestServices.GetService<IJwtService>();
            var cookieOptions = context.HttpContext.RequestServices.GetService<IOptions<ICookieOptions>>()?.Value;
            var token = context.HttpContext.Request.Cookies[cookieOptions!.JwtToken];
            if (string.IsNullOrEmpty(token))
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    Status = "error",
                    Result = "Not found token"
                });
                return;
            }
            var tokenPayload = jwtService!.VerifyToken(token);
            if (tokenPayload == null)
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    Status = "error",
                    Result = "Bad token"
                });
                return;
            }
            context.HttpContext.Items[cookieOptions.JwtToken] = tokenPayload;
        }
    }
}