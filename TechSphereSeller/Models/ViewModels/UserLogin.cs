using FluentValidation;

namespace TopShopSeller.Models
{
    public class UserLogin
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

    }

    public class LoginValidator : AbstractValidator<UserLogin>
    {
        public LoginValidator()
        {
            RuleFor(u => u.Username).NotEmpty().NotNull();
            RuleFor(u => u.Password).MinimumLength(5);
        }
    }
}
