using System.Collections.Generic;
using System.Threading.Tasks;
using TopShopBuyer.Models;

namespace TopShopBuyer.DataLayer
{
    public interface IProducts
    {
        Task<List<Product>> AllProducts();
        Task<List<Product>> ProductsByCategory(string category);
        Task<bool> AddToCart(Cart cart);
        Task<bool> Checkout(int cartId);
        Task<bool> RemoveCartProduct(int productId, int cartId);
        Task<Cart> GetCart();
        Task<List<Cart>> GetAllCarts();

        Task<List<Cart>> GetAllActiveCarts();

    }
}
