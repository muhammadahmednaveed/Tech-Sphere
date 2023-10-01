namespace TopShopSeller.Models
{
    public class ProductInventory
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public int Inventory { get; set; } = 0;
    }
}
