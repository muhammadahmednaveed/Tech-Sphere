using System.IdentityModel.Tokens.Jwt;

namespace TopShopSeller
{
    public class TokenValidator
    {
        private readonly RequestDelegate _next;

        public TokenValidator(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                var authorizationHeader = httpContext.Request.Headers["Authorization"].ToString();
                if (!string.IsNullOrEmpty(authorizationHeader))
                {
                    var jwtEncodedString = authorizationHeader[7..];
                    var token = new JwtSecurityToken(jwtEncodedString: jwtEncodedString);
                    if (DateTime.Compare(DateTime.UtcNow, token.ValidTo) > 0)
                    {
                        httpContext.Response.Clear();
                        httpContext.Response.StatusCode = 440;
                        await httpContext.Response.WriteAsync("Login has been expired.");
                    }
                }
                await _next(httpContext);
            }
            catch
            {
                httpContext.Response.StatusCode = 505;
                await httpContext.Response.WriteAsync("An error occurred while processing your request.");
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class TokenValidatorExtensions
    {
        public static IApplicationBuilder UseTokenValidator(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenValidator>();
        }
    }
}
