using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Threading.Tasks;
using TopShopBuyer.Models;

namespace TopShopBuyer.DataLayer
{
    public class Products:IProducts
    {
        string connectionString = "";                               //change connecion string according to your server and database

        private IHttpContextAccessor _context { get; }

        public Products(IConfiguration config, IHttpContextAccessor context)
        {
            connectionString = config.GetConnectionString("ProjectDB");
            _context = context;
        }

        public async Task<List<Product>> AllProducts()
        {
            List<Product> allProducts = new();
            

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetAllProducts", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    await con.OpenAsync();
                    SqlDataReader sdr = await cmd.ExecuteReaderAsync();

                    while (await sdr.ReadAsync())
                    {
                        Product product = new();
                        product.Id = Convert.ToInt32(sdr[0]);
                        product.Title = sdr[1].ToString();
                        product.Description = sdr[2].ToString();
                        product.Category = sdr[3].ToString();
                        product.Price = (double)sdr[4];
                        product.Inventory = Convert.ToInt32(sdr[5]);
                        product.ImageUrl = sdr[6].ToString();
                        product.Rating = (double)sdr[7];
                        product.SellerId = Convert.ToInt32(sdr[8]);
                        product.Quantity = 0;
                        allProducts.Add(product);
                    }
                    sdr.Close();
                }
                return allProducts;
            }     
        }

        public async Task<List<Product>> ProductsByCategory(string category)
        {
            List<Product> allProducts = new();
            int postID = 0;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetProductsByCategory", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@category", category);
                    await con.OpenAsync();
                    SqlDataReader sdr = await cmd.ExecuteReaderAsync();

                    while (await sdr.ReadAsync())
                    {
                        Product product = new();
                        product.Id = Convert.ToInt32(sdr[0]);
                        product.Title = sdr[1].ToString();
                        product.Description = sdr[2].ToString();
                        product.Category = sdr[3].ToString();
                        product.Price = (double)sdr[4];
                        product.Inventory = Convert.ToInt32(sdr[5]);
                        product.ImageUrl = sdr[6].ToString();
                        product.Rating = (double)sdr[7];
                        product.SellerId = Convert.ToInt32(sdr[8]);
                        allProducts.Add(product);
                    }
                }
                return allProducts;
            }
        }


        public async Task<bool> AddToCart(Cart cart)
        {
            using (SqlConnection con = new SqlConnection(connectionString))                     //making sql connection
            {
                using (SqlCommand cmd = new SqlCommand("sp_AddToCart", con))
                {
                    cart.BuyerId = await GetBuyerID();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@buyerId", cart.BuyerId);                   
                                     
                    cmd.Parameters.AddWithValue("@productId", cart.ProductID);                   
                    cmd.Parameters.AddWithValue("@quantity", cart.ProductQuantity);                                   


                    await con.OpenAsync();                                                 //opening the connection
                    int affectedRows = await cmd.ExecuteNonQueryAsync();                                      //executing the query
                    return affectedRows>0;
                    
                }
                
            }
        }
        public async Task<bool> Checkout(int cartId)
        {

            //DataTable quantityList = new DataTable("Products");
            //DataColumn Columns;
            //DataRow Row;
            //Columns = new DataColumn();
            //Columns.DataType = typeof(int);
            //Columns.ColumnName = "ProductId";
            //quantityList.Columns.Add(Columns);
            //Columns = new DataColumn();
            //Columns.DataType = typeof(int);
            //Columns.ColumnName = "Quantity";
            //quantityList.Columns.Add(Columns);

            //foreach(var product in cart.Products)
            //{
            //    Row = quantityList.NewRow();
            //    Row["ProductId"] = product.Id;
            //    Row["Quantity"] = product.Quantity;
            //    quantityList.Rows.Add(Row);
            //}

            using (SqlConnection con = new SqlConnection(connectionString))                     //making sql connection
            {
                using (SqlCommand cmd = new SqlCommand("sp_checkout", con))
                {
                    int buyerId = await GetBuyerID();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@buyerId", buyerId);
                    cmd.Parameters.AddWithValue("@cartId", cartId);



                    //var param = new SqlParameter()
                    //{
                    //    SqlDbType = SqlDbType.Structured, //enum
                    //    TypeName = "dbo.ProductQuantityList", //Type name at SQL
                    //    Value = quantityList, //Datatable in Code
                    //    ParameterName = "@products" // sp parameter name
                    //};
                    //cmd.Parameters.Add(param);



                    await con.OpenAsync();                                                 //opening the connection
                    int affectedRows = await cmd.ExecuteNonQueryAsync();                                      //executing the query
                    return affectedRows > 0;

                }

            }
        }
        public async Task<bool> RemoveCartProduct(int productId,int cartId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))                     //making sql connection
            {
                using (SqlCommand cmd = new SqlCommand("sp_removefromcart", con))
                {
                    int buyerId = await GetBuyerID();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@buyerId", buyerId);                  
                    cmd.Parameters.AddWithValue("@productId", productId);
                    cmd.Parameters.AddWithValue("@cartId", cartId);



                    await con.OpenAsync();                                                 //opening the connection
                    var rowsDeleted = await cmd.ExecuteNonQueryAsync();                                      //executing the query
                    return rowsDeleted > 0;
                }

            }
        }
        public async Task<Cart> GetCart()
        {
            Cart cart = new();

            using (SqlConnection con = new SqlConnection(connectionString))
            {

                using (SqlCommand cmd = new SqlCommand("sp_getcart", con))
                {
                    cart.BuyerId = await GetBuyerID();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@buyerId", cart.BuyerId);
                    await con.OpenAsync();
                    SqlDataReader sdr = await cmd.ExecuteReaderAsync();

                    while (sdr.Read())
                    {
                        Product product = new();
                        product.Id = Convert.ToInt32(sdr[5]);
                        product.Title = sdr[6].ToString();
                        product.Description = sdr[7].ToString();
                        product.Category = sdr[8].ToString();
                        product.Price = (double)sdr[9];
                        product.Inventory = Convert.ToInt32(sdr[10]);
                        product.ImageUrl = sdr[11].ToString();
                        product.Rating = (double)sdr[12];
                        product.SellerId = Convert.ToInt32(sdr[13]);
                        product.Quantity = Convert.ToInt32(sdr[3]);
                        cart.CartId = Convert.ToInt32(sdr[1]);
                        cart.Products.Add(product);
                    }
                    sdr.Close();
                }
                return cart;
            }
        }

        public async Task<List<Cart>> GetAllCarts()
        {
            List<Cart> allCarts = new List<Cart>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {

                using (SqlCommand cmd = new SqlCommand("sp_getallcart", con))
                {
                    int buyerId = await GetBuyerID();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@buyerId", buyerId);
                    await con.OpenAsync();
                    SqlDataReader sdr = await cmd.ExecuteReaderAsync();

                    Cart cart = new Cart();
                    int cartId = 0; 

                    while (await sdr.ReadAsync())
                    {
                        if (!(cartId == Convert.ToInt32(sdr[1])))
                        {
                            if (cartId != 0)
                            {
                                allCarts.Add(cart);
                                cart = new Cart();
                            }
                            
                            
                            cartId = Convert.ToInt32(sdr[1]);
                        }

                        Product product = new();
                        product.Id = Convert.ToInt32(sdr[5]);
                        product.Title = sdr[6].ToString();
                        product.Description = sdr[7].ToString();
                        product.Category = sdr[8].ToString();
                        product.Price = (double)sdr[9];
                        product.Inventory = Convert.ToInt32(sdr[10]);
                        product.ImageUrl = sdr[11].ToString();
                        product.Rating = (double)sdr[12];
                        product.SellerId = Convert.ToInt32(sdr[13]);
                        product.Quantity = Convert.ToInt32(sdr[3]);
                        cart.CartId = Convert.ToInt32(sdr[1]);
                        cart.PurchaseDate = Convert.ToDateTime(sdr[20]);
                        cart.TotalPrice = Convert.ToInt32(sdr[19]);
                        cart.Products.Add(product);
                    }
                    sdr.Close();
                    allCarts.Add(cart);
                }
                
                return allCarts;
            }
        }
        public async Task<int> GetBuyerID()
        {
            if (_context.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                int id = Convert.ToInt32(_context.HttpContext.User.FindFirstValue(ClaimTypes.SerialNumber));
                return id;
            }
            else
            {
                return 1;
            }
                
            
            
        }

        public async Task<List<Cart>> GetAllActiveCarts()
        {
            List<Cart> allCarts = new List<Cart>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                using (SqlCommand cmd = new SqlCommand("sp_GetAllActiveCarts", con))
                {
                    
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    await con.OpenAsync();
                    SqlDataReader sdr = await cmd.ExecuteReaderAsync();

                    Cart cart = new Cart();
                    int cartId = 0;

                    while (await sdr.ReadAsync())
                    {
                        if (!(cartId == Convert.ToInt32(sdr[1])))
                        {
                            if (cartId != 0)
                            {
                                allCarts.Add(cart);
                                cart = new Cart();
                            }


                            cartId = Convert.ToInt32(sdr[1]);
                        }

                        Product product = new();
                        product.Id = Convert.ToInt32(sdr[5]);
                        cart.CartId = Convert.ToInt32(sdr[1]);
                        cart.BuyerId = Convert.ToInt32(sdr[18]);
                        cart.Products.Add(product);
                    }
                    sdr.Close();
                    allCarts.Add(cart);
                }

                return allCarts;
            }
        }
    }

   
}
