using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopShopSeller.Core;
using TopShopSeller.Models;

namespace TopShopSeller.Controllers
{
    [Authorize(Roles = "seller")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly string _requestLocation;
        private readonly IValidator<Product> _productValidator;

        public ProductsController(IProductService productService, IHttpContextAccessor HttpContext, IValidator<Product> productValidator)
        {
            _productService = productService;
            _requestLocation = HttpContext.HttpContext.Request.Path;
            _productValidator = productValidator;
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
                    var product = await _productService.GetProductById(productId);
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
                ValidationResult result = await _productValidator.ValidateAsync(product);
                var editedProduct = await _productService.EditProduct(product, productId);
                if (!result.IsValid)
                {
                    return BadRequest(result);
                }
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
                var deleted = await _productService.DeleteProduct(productId);
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
                ValidationResult result = await _productValidator.ValidateAsync(product);
                var newProduct = await _productService.AddProduct(product, sellerId);
                if (!result.IsValid)
                {
                    return BadRequest(result);
                }
                return Created(_requestLocation, newProduct);
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
                    var productCount = await _productService.GetSellerProductCount(sellerId);
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
                    var products = await _productService.GetSellerProducts(pageNumber, pageSize, sellerId);
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
                    var productInventory = await _productService.GetInventoryById(productId);
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
                var edited = await _productService.EditInventory(productId, inventory);
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
