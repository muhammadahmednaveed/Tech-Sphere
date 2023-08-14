using JWTAuthentication.DataLayer;
using JWTAuthentication.Models;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MyProject.DataLayer
{
    public class DBHelper : IDBHelper
    {
        //This configuration is added to access the AppSettings

        
        private readonly string connectionString = "";
        public DBHelper(IConfiguration configuration)
        {

            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<bool> Register(UserRegister user)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                using (SqlCommand cmd = new SqlCommand("sp_RegisterUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    Password.CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);

                    cmd.Parameters.AddWithValue("@fullName", user.Fullname);
                    cmd.Parameters.AddWithValue("@username", user.Username);
                    cmd.Parameters.AddWithValue("@email", user.Email);
                    cmd.Parameters.AddWithValue("@passwordHash", passwordHash);
                    cmd.Parameters.AddWithValue("@passwordSalt", passwordSalt);
                    cmd.Parameters.AddWithValue("@userType", user.UserType);

                    await con.OpenAsync();                                                 //opening the connection
                    int affectedRows = await cmd.ExecuteNonQueryAsync();                                      //executing the query
                    return affectedRows > 0;
                }
            }
        }

        public async Task<User> Login(UserLogin loginUser)
        {
            User currentUser = new User();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@userName", loginUser.Username));

                    con.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();
                    if (sdr.Read())
                    {
                        currentUser.UserId = sdr.GetInt32(0);
                        currentUser.Fullname = sdr.GetString(1);
                        currentUser.Username = sdr.GetString(2);
                        currentUser.Email = sdr.GetString(3);
                        currentUser.UserType = sdr.GetString(6);
                        loginUser.PasswordHash = (byte[])sdr[4];
                        loginUser.PasswordSalt = (byte[])sdr[5];
                    }
                    else
                    {
                        return new User();
                    }
                }

                if (!Password.VerifyPasswordHash(loginUser.Password, loginUser.PasswordHash, loginUser.PasswordSalt))
                {
                    return (new User() { UserId=-1});
                }

                return currentUser;
            }
        }



    }
}