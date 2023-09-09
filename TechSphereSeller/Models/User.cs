namespace TopShopSeller.Models
{
    public class User
    {
        public int? UserId { get; set; }
        public string Fullname { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserType { get; set; } = "buyer";
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
