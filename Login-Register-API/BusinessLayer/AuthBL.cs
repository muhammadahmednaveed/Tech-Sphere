using JWTAuthentication.DataLayer;
using JWTAuthentication.Models;
using Microsoft.IdentityModel.Tokens;
using MyProject.DataLayer;
using Nito.AsyncEx;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTAuthentication.BusinessLayer
{
    public class AuthBL:IAuthBL
    {
        private readonly IDBHelper _db;
        private readonly ITokenHelper _token;
        private readonly IConfiguration _config;


        public AuthBL(IDBHelper db,ITokenHelper token)
        {
            _db = db;
            _token = token;
        }

        public async Task<User> Login(UserLogin user)
        {
            var loggedUser = await _db.Login(user);
            if(loggedUser.UserId!=null)
            {
                loggedUser.Token = CreateToken(loggedUser);
                return loggedUser;
            }
            return loggedUser;

        }



        public async Task<bool> Register(UserRegister user)
        {
            bool regUser = await _db.Register(user);
            return regUser;

        }
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.SerialNumber, user.UserId.ToString()),
                new Claim(ClaimTypes.Name,user.Username),
                new Claim(ClaimTypes.Role, user.UserType)
            };

            Dictionary<int, string> keyDict = _token.SigningKeyList();
            List<int> keyDictIds = new List<int>(keyDict.Keys);
            Random rand = new Random();
            int randomKeyId = keyDictIds[rand.Next(keyDictIds.Count)];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                keyDict[randomKeyId]));

            key.KeyId = randomKeyId.ToString();
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: cred);
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }
    }
}
