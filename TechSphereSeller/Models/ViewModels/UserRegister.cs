using FluentValidation;

namespace TopShopSeller.Models
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

    public class RegistrationValidator : AbstractValidator<UserRegister>
    {
        public RegistrationValidator()
        {
            RuleFor(u => u.Fullname).NotNull().NotEmpty();
            RuleFor(u => u.Username).NotNull().NotEmpty();
            RuleFor(u => u.Email).EmailAddress();
            RuleFor(u => u.Password).MinimumLength(5);
            RuleFor(u => u.UserType).NotNull().NotEmpty();
        }
    }
}
