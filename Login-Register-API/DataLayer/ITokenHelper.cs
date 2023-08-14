using JWTAuthentication.Models;

namespace JWTAuthentication.DataLayer
{
    public interface ITokenHelper
    {

       Dictionary<int, string> SigningKeyList();

        void ValidateToken(string token);

       void BlackListToken(string token);
       
    }
}
