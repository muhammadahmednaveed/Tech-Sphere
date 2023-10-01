using System.Data;
using System.Data.SqlClient;
using TopShopSeller.Models;

namespace TopShopSeller.Core
{
    public class ProductService : IProductService
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString = string.Empty;
        public ProductService(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetSection("ConnectionStrings:DefaultConnection").Value;
        }


        /// <summary>
        /// It takes an object and a seller Id to add a product in the database
        /// </summary>
        /// <param name="product"></param>
        /// <param name="sellerId"></param>
        /// <returns></returns>
        public async Task<Product> AddProduct(Product product, int sellerId)
        {
            using SqlConnection con = new(connectionString);
            await con.OpenAsync();
            using SqlCommand cmd = new("sp_AddProduct", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@title", product.Title);
            cmd.Parameters.AddWithValue("@description", product.Description);
            cmd.Parameters.AddWithValue("@category", product.Category);
            cmd.Parameters.AddWithValue("@price", product.Price);
            cmd.Parameters.AddWithValue("@inventory", product.Inventory);
            cmd.Parameters.AddWithValue("@imageUrl", product.ImageUrl);
            cmd.Parameters.AddWithValue("@sellerId", sellerId);
            await cmd.ExecuteNonQueryAsync();
            return product;
        }


        /// <summary>
        /// It takes an object of Product and product Id to edit the product
        /// </summary>
        /// <param name="product"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Product> EditProduct(Product product, int id)
        {
            using SqlConnection con = new(connectionString);
            await con.OpenAsync();
            using SqlCommand cmd = new("sp_EditProduct", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@productId", id);
            cmd.Parameters.AddWithValue("@title", product.Title);
            cmd.Parameters.AddWithValue("@description", product.Description);
            cmd.Parameters.AddWithValue("@category", product.Category);
            cmd.Parameters.AddWithValue("@price", product.Price);
            cmd.Parameters.AddWithValue("@inventory", product.Inventory);
            cmd.Parameters.AddWithValue("@imageUrl", product.ImageUrl);
            int affectedRows = await cmd.ExecuteNonQueryAsync();

            if (affectedRows != 0)
            {
                return product;
            };
            return new Product();
        }


        /// <summary>
        /// It only takes product id as a parameter to delete the product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteProduct(int id)
        {
            using SqlConnection con = new(connectionString);
            await con.OpenAsync();
            using SqlCommand cmd = new("sp_DeleteProduct", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@productId", id);
            int affectedRows = await cmd.ExecuteNonQueryAsync();

            if (affectedRows != 0) return true;
            return false;
        }


        /// <summary>
        /// It uses seller id to get the total products count
        /// </summary>
        /// <param name="sellerId"></param>
        /// <returns></returns>
        public async Task<int> GetSellerProductCount(int sellerId)
        {
            using SqlConnection con = new(connectionString);
            await con.OpenAsync();
            using SqlCommand cmd = new("sp_GetProductCount", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SellerId", sellerId);
            using SqlDataReader sdr = await cmd.ExecuteReaderAsync();

            int productCount = 0;
            if (await sdr.ReadAsync())
            {
                productCount = Convert.ToInt32(sdr[0]);
            }
            return productCount;
        }


        /// <summary>
        /// It takes three parameters to get seller products with pagination
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="sellerId"></param>
        /// <returns></returns>
        public async Task<List<SellerProduct>> GetSellerProducts(int pageNumber, int pageSize, int sellerId)
        {
            List<SellerProduct> sellerProductsList = new();
            using SqlConnection con = new(connectionString);
            await con.OpenAsync();
            using SqlCommand cmd = new("sp_GetProductsWithPagination", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            cmd.Parameters.AddWithValue("@SellerId", sellerId);
            using SqlDataReader sdr = await cmd.ExecuteReaderAsync();

            while (await sdr.ReadAsync())
            {
                SellerProduct product = new();
                product.Id = Convert.ToInt32(sdr["productId"]);
                product.Title = sdr["title"].ToString();
                product.Description = sdr["description"].ToString();
                product.Category = sdr["category"].ToString();
                product.Price = (double)(sdr["price"]);
                product.Inventory = Convert.ToInt32(sdr["inventory"]);
                product.ImageUrl = sdr["imageUrl"].ToString();
                product.Rating = (double)(sdr["rating"]);
                sellerProductsList.Add(product);
            }
            return sellerProductsList;
        }


        /// <summary>
        /// It takes product Id as parameter to return the product object
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<SellerProduct> GetProductById(int productId)
        {
            using SqlConnection con = new(connectionString);
            await con.OpenAsync();
            using SqlCommand cmd = new("sp_GetProductById", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@productId", productId);
            using SqlDataReader sdr = await cmd.ExecuteReaderAsync();
            SellerProduct product = new();

            if (await sdr.ReadAsync())
            {
                product.Id = Convert.ToInt32(sdr["productId"]);
                product.Title = sdr["title"].ToString();
                product.Description = sdr["description"].ToString();
                product.Category = sdr["category"].ToString();
                product.Price = (double)(sdr["price"]);
                product.Inventory = Convert.ToInt32(sdr["inventory"]);
                product.ImageUrl = sdr["imageUrl"].ToString();
                product.Rating = (double)sdr["rating"];
                return product;
            }
            return null;
        }


        /// <summary>
        /// it takes product Id as parameter and retrun the inventory details of that product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<ProductInventory> GetInventoryById(int productId)
        {
            using SqlConnection con = new(connectionString);
            await con.OpenAsync();
            using SqlCommand cmd = new("sp_GetInventorById", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@productId", productId);
            using SqlDataReader sdr = await cmd.ExecuteReaderAsync();
            ProductInventory productInventory = new();

            if (await sdr.ReadAsync())
            {
                productInventory.Id = Convert.ToInt32(sdr["productId"]);
                productInventory.Title = sdr["title"].ToString();
                productInventory.Inventory = Convert.ToInt32(sdr["inventory"]);
                return productInventory;
            }
            return null;
        }


        /// <summary>
        /// it takes product Id and new inventory value as parameters to update the inventory of that product
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="inventory"></param>
        /// <returns></returns>
        public async Task<bool> EditInventory(int productId, int inventory)
        {
            using SqlConnection con = new(connectionString);
            await con.OpenAsync();
            using SqlCommand cmd = new("sp_EditInventory", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@productId", productId);
            cmd.Parameters.AddWithValue("@inventory", inventory);
            int affectedRows = await cmd.ExecuteNonQueryAsync();

            if (affectedRows != 0) return true;
            return false;
        }
    }
}
