using JWTAuthentication.Models;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTAuthentication.DataLayer
{
    public class TokenHelper:ITokenHelper
    {

        private static string connectionString = "";
        public TokenHelper(IConfiguration configuration)
        {
            //connectionString = configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
       



        public Dictionary<int, string> SigningKeyList()
        {
            Dictionary<int, string> allKeys = new Dictionary<int, string>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_getAllKeys", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();

                    while (sdr.Read())
                    {
                        allKeys[sdr.GetInt32("id")] = sdr.GetString("keys");
                    }

                    return allKeys;
                }
            }

        }

        

        public async void ValidateToken(string token)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_rejectedTokenList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;


                    await con.OpenAsync();
                    SqlDataReader sdr = await cmd.ExecuteReaderAsync();

                    while (await sdr.ReadAsync())
                    {

                        string blacklisttoken = sdr.GetString(0);
                        if (blacklisttoken == token)
                        {
                            throw new SecurityTokenException("Blacklisted");
                        }
                    }
                }
                

                
            }
        }
        public async void BlackListToken(string token)
        {
            //string auth = HttpContext.Request.Headers["Authorization"];
            //string[] authArr = new string[2];
            //authArr = auth.Split(" ");
            //string token = authArr[1];

            using (SqlConnection con = new SqlConnection(connectionString))                   
            {
                using (SqlCommand cmd = new SqlCommand("sp_addRejectedToken", con))
                {
                    //giving the stored procedure to run
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@tokens", token);                  
                    await con.OpenAsync();                                                
                    await cmd.ExecuteNonQueryAsync();                                    
                }

            }
        }
    }
}
