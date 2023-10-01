using TopShopSeller.Models;

namespace TopShopSeller.Core
{
    public interface IProductService
    {
        Task<Product> AddProduct(Product product, int sellerId);

        Task<Product> EditProduct(Product product, int id);

        Task<bool> DeleteProduct(int id);

        Task<int> GetSellerProductCount(int sellerId);

        Task<List<SellerProduct>> GetSellerProducts(int pageNumber, int pageSize, int sellerId);

        Task<SellerProduct> GetProductById(int productId);

        Task<ProductInventory> GetInventoryById(int productId);

        Task<bool> EditInventory(int productId, int inventory);
    }
}
