using JWTAuthentication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MyProject.DataLayer
{
    public interface IDBHelper
    {
        Task<bool> Register(UserRegister user);
        Task<User> Login(UserLogin user);


    }
}
