namespace JWTAuthentication.Models
{
    public class UserRegister
    {
        public int? UserId { get; set; } = 0;
        public string Fullname { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string UserType { get; set; } = "buyer";
    }
}
