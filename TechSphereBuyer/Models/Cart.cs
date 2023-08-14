using System;
using System.Collections.Generic;
using TopShopBuyer.DataLayer;

namespace TopShopBuyer.Models
{
    public class Cart
    {
        public int CartId { get; set; } = 1;
        public int BuyerId { get; set; } = 1;
        public int ProductID { get; set; } = 1;
        public int ProductQuantity { get; set; } = 1;
        public double TotalPrice { get; set; } = 0;
        public List<Product> Products { get; set; } = new();
        public DateTime PurchaseDate { get; set; }= DateTime.Now;
    }
}
