using FluentValidation;

namespace TopShopSeller.Models
{
    public class Product
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public double Price { get; set; } = 0;

        public int Inventory { get; set; } = 0;

        public string ImageUrl { get; set; } = string.Empty;
    }

    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(u => u.Title).MinimumLength(10);
            RuleFor(u => u.Category).NotNull().NotEmpty();
            RuleFor(u => u.Price).GreaterThanOrEqualTo(0);
            RuleFor(u => u.Inventory).GreaterThanOrEqualTo(0);
        }
    }
}
