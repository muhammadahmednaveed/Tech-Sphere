using JWTAuthentication.Models;
using Microsoft.AspNetCore.Mvc;

namespace MyProject.DataLayer
{
    public interface IDBHelper
    {
        Task<bool> Register(UserRegister user);
        Task<User> Login(UserLogin user);


    }
}
