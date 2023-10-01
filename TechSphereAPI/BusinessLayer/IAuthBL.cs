using JWTAuthentication.Models;
using System.Threading.Tasks;

namespace JWTAuthentication.BusinessLayer
{
    public interface IAuthBL
    {
        Task<bool> Register(UserRegister user);
        Task<User> Login(UserLogin user);
    }
}
