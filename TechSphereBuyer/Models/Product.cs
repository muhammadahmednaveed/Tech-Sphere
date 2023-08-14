namespace TopShopBuyer.Models
{
    public class Product
    {
        public int Id { get; set; } = 0;
        public string Title { get; set; } = string.Empty; 
        public string Description { get; set; } = string.Empty; 
        public string Category { get; set; } = string.Empty; 
        public double Price { get; set; } = 0; 
        public int Inventory { get; set; } = 0; 
        public string ImageUrl { get; set; } = string.Empty;
        public double Rating { get; set; } = 0;
        public int SellerId { get; set; } = 1;
        public int Quantity { get; set; } = 0;
    }
}

