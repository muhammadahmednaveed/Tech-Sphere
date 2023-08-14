using JWTAuthentication.DataLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using MyProject.DataLayer;
using Nito.AsyncEx;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTAuthentication.Validators
{
    public class JWTValidator : JwtSecurityTokenHandler
    {
        private static IConfiguration configuration;

        TokenHelper _token = new TokenHelper(configuration);
        public override ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            var token2 = ReadJwtToken(token);
            int kid = Convert.ToInt32(token2.Header.Kid);

            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetSecret(kid)));

            _token.ValidateToken(token);

            //if (context.HttpContext.Request.Path.Value.StartsWith("/logout"))
            //{
            //    data.BlackListToken(token);

            //}
            

            return base.ValidateToken(token, validationParameters, out validatedToken);

            //}


        }

        public string GetSecret(int kid)
        {
            var secretList =  _token.SigningKeyList();
            string secret = secretList[kid];
            return secret;
        }



    }
}
