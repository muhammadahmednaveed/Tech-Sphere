
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TopShopBuyer.DataLayer;
using TopShopBuyer.Models;

namespace TopShopSeller.Controllers
{
    [Authorize(Roles = "Seller")]
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly SellerDataAccess _sellerDataAccess;

        public ProductsController(SellerDataAccess sellerDataAccess)
        {
            _sellerDataAccess = sellerDataAccess;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{productId}")]
        public async Task<IActionResult> GetProductById([FromRoute] int productId)
        {
            try
            {
                if (productId != 0)
                {
                    var product = await _sellerDataAccess.GetProductById(productId);
                    if (product == null)
                    {
                        return NotFound("Product does not exist.");
                    }
                    return Ok(product);
                }
                return BadRequest("Invalid product ID.");
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpPut]
        [Route("{productId}")]
        public async Task<IActionResult> EditProduct([FromBody] Product product, [FromRoute] int productId)
        {
            try
            {
                var editedProduct = await _sellerDataAccess.EditProduct(product, productId);
                return Ok(editedProduct);
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpDelete]
        [Route("{productId}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int productId)
        {
            try
            {
                var deleted = await _sellerDataAccess.DeleteProduct(productId);
                if (!deleted)
                {
                    return NotFound("Product does not exist.");
                }
                return Ok("Product has been deleted successfully.");
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] Product product, [FromQuery] int sellerId)
        {
            try
            {
                var newProduct = await _sellerDataAccess.AddProduct(product, sellerId);
                return Ok(newProduct);
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet]
        [Route("seller/count/{sellerId}")]
        public async Task<IActionResult> GetSellerProductCount([FromRoute] int sellerId)
        {
            try
            {
                if (sellerId > 0)
                {
                    var productCount = await _sellerDataAccess.GetSellerProductCount(sellerId);
                    return Ok(productCount);
                }
                return BadRequest("Invalid seller Id");
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpGet]
        [Route("seller/{sellerId}")]
        public async Task<IActionResult> GetSellerProducts([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromRoute] int sellerId)
        {
            try
            {
                if (pageNumber > 0 && pageSize > 0 && sellerId > 0)
                {
                    var products = await _sellerDataAccess.GetSellerProducts(pageNumber, pageSize, sellerId);
                    if (products.Count <= 0)
                    {
                        return NoContent();
                    }
                    return Ok(products);
                }
                return BadRequest("Invalid parameter");
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpGet]
        [Route("inventory/{productId}")]
        public async Task<IActionResult> GetInventoryById([FromRoute] int productId)
        {
            try
            {
                if (productId > 0)
                {
                    var productInventory = await _sellerDataAccess.GetInventoryById(productId);
                    if (productInventory == null)
                    {
                        return NotFound("Product does not exist.");
                    }
                    return Ok(productInventory);
                }
                return BadRequest("Invalid product ID.");
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpPut]
        [Route("inventory/{productId}")]
        public async Task<IActionResult> EditInventory([FromRoute] int productId, [FromQuery] int inventory)
        {
            try
            {
                var edited = await _sellerDataAccess.EditInventory(productId, inventory);
                if (!edited)
                {
                    return NotFound("Product does not exist.");
                }
                return Ok("Product inventory has been updated.");
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
