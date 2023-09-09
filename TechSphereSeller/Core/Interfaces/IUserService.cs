using TopShopSeller.Models;

namespace TopShopSeller.Core
{
    public interface IUserService
    {
        Task<UserRegister> Register(UserRegister user);
        Task<string> Login(UserLogin user);
        Task<User> GetUserByUserName(UserLogin user);
        public string CreateToken(User user);
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}
