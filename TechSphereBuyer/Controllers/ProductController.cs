using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TopShopBuyer.DataLayer;
using TopShopBuyer.Models;
using Microsoft.AspNetCore.Authorization;
using Serilog.Context;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace TopShopBuyer.Controllers
{
    [Authorize (Policy= "LoginSecure")]
    [ApiController]
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;

        private readonly IProducts product;

        private readonly string? siteUrl;

        public ProductController(ILogger<ProductController> logger, IProducts product, IHttpContextAccessor context)
        {
            _logger = logger;
            this.product = product;
            this.siteUrl = context.HttpContext.Request.Path;
        }
        //[AllowAnonymous]
        [Route("allproducts")]
        [HttpGet]
        public async Task<IActionResult> AllProducts()
        {

            //using(LogContext.PushProperty("username", HttpContext.User.Claims.Where(x=>x.Type==ClaimTypes.Name).First().Value))
            try
            {
                _logger.LogInformation("Inside All Products");
                return Ok(await product.AllProducts());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,ex.Message);
                return Problem(ex.Message, null, 500);
            }
        }
        
        [Route("allproducts/{category}")]
        [HttpGet]
        public async Task<IActionResult> ProductsByCategory(string category)
        {
            try
            {
                return Ok(await product.ProductsByCategory(category));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Problem(ex.Message, null, 500);
            }
        }

        [Route("addtocart")]
        [HttpPost]
        public async Task<IActionResult> AddToCart(Cart cart)
        {
            try
            {
                
                bool added = await product.AddToCart(cart);
                if (added)
                {
                    return Created(siteUrl, "Product added to cart");
                }
                else
                {
                    _logger.LogWarning("No rows affected while adding to cart");
                    return BadRequest("Product not added to cart");
                }
                

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Problem(ex.Message,null,500);
            }
        }
        [Route("checkout/{cartId}")]
        [HttpPut]
        public async Task<IActionResult> Checkout(int cartId)
        {
            try
            {
                bool checkout = await product.Checkout(cartId);
                if (checkout)
                {
                    return Ok("Your purchase has been successful");
                }
                else
                {
                    _logger.LogWarning("No rows affected while checkout");
                    return BadRequest("Cannot complete the purchase.Maybe the cart does not exist or purchase has been completed already");
                }
                

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Problem(ex.Message, null, 500);
            }
        }
        [Route("removecartproduct/{productId}")]
        [HttpPut]
        public async Task<IActionResult> RemoveCartProduct(int productId,int cartId)
        {
            try
            {
                bool updated = await product.RemoveCartProduct(productId,cartId);
                if(updated)
                {
                    return Ok("Product Removed from Cart");
                }
                else
                {
                    _logger.LogWarning("No rows affected while removing from cart");
                    return BadRequest("Some error occcured");
                }
                

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Problem(ex.Message, null, 500);
            }
        }
        [Route("getcart")]
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            try
            {
                
                return Ok(await product.GetCart());

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Problem(ex.Message, null, 500);
            }
        }
        [Route("getallcart")]
        [HttpGet]
        public async Task<IActionResult> GetAllCart()
        {
            try
            {
                List<Cart> carts = await product.GetAllCarts();
                return Ok(carts);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Problem(ex.Message, null, 500);
            }
        }


    }
}
