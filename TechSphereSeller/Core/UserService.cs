using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TopShopSeller.Models;

namespace TopShopSeller.Core
{
    public class UserService : IUserService
    {
        //This configuration is added to access the AppSettings
        private readonly IConfiguration _configuration;
        private readonly string connectionString = string.Empty;
        public UserService(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetSection("ConnectionStrings:DefaultConnection").Value;
        }

        public async Task<UserRegister> Register(UserRegister user)
        {
            using SqlConnection con = new(connectionString);
            using SqlCommand cmd = new("sp_RegisterUser", con);
            cmd.CommandType = CommandType.StoredProcedure;
            CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
            cmd.Parameters.AddWithValue("@fullName", user.Fullname);
            cmd.Parameters.AddWithValue("@username", user.Username);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@passwordHash", passwordHash);
            cmd.Parameters.AddWithValue("@passwordSalt", passwordSalt);
            cmd.Parameters.AddWithValue("@userType", user.UserType);
            await con.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return user;
        }

        public async Task<string> Login(UserLogin user)
        {
            User currentUser = await GetUserByUserName(user);
            if (!currentUser.Username.IsNullOrEmpty())
            {
                if (!VerifyPasswordHash(user.Password, currentUser.PasswordHash, currentUser.PasswordSalt)) return ("Invalid Password!");
                string authToken = CreateToken(currentUser);
                return authToken;
            }
            return "User Not Found!";
        }

        public async Task<User> GetUserByUserName(UserLogin user)
        {
            User currentUser = new();
            using SqlConnection con = new(connectionString);
            using SqlCommand cmd = new("sp_GetUser", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@userName", user.Username));
            await con.OpenAsync();
            using SqlDataReader sdr = await cmd.ExecuteReaderAsync();

            if (sdr.Read())
            {
                currentUser.UserId = sdr.GetInt32("UserId");
                currentUser.Username = sdr.GetString("Username");
                currentUser.UserType = sdr.GetString("UserType");
                currentUser.PasswordHash = (byte[])sdr["PasswordHash"];
                currentUser.PasswordSalt = (byte[])sdr["PasswordSalt"];
            }
            return currentUser;
        }


        public string CreateToken(User user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.SerialNumber, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.UserType)
            };

            //Creating an auth. token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: cred);
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }


        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }


        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            //Comparing both hashes byte by byte
            return computedHash.SequenceEqual(passwordHash);
        }
    }
}