
using Microsoft.AspNetCore.Http;

namespace Presentation
{
    [Authorize(Roles = "Admin")]
    public class DashboardController(IServiceManager serviceManager)
        : ApiController
    {
        [HttpPost("add-product")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddProduct([FromForm] CreateProductDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new
            {
                StatusCode = 401,
                ErrorMessage = "User not authenticated."
            });

            await serviceManager.DashboardService.AddProductAsync(userId, dto);
            return Ok("Product added successfully");
        }

        [HttpPut("update-product/{productId}")]
        public async Task<IActionResult> UpdateProblem(string productId, [FromBody] UpdateProductDto dto)
        {
            await serviceManager.DashboardService.UpdateProductAsync(productId, dto);
            return Ok("Product updated successfully");
        }
        
        [HttpPut("update-product-image/{productId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateProductImage(string productId, [FromForm] IFormFile image)
        {
            await serviceManager.DashboardService.UpdateProductImageAsync(productId, image);
            return Ok("Product image updated successfully");
        }
        
        [HttpDelete("delete-product/{productId}")]
        public async Task<IActionResult> DeleteProduct(string productId)
        {
            await serviceManager.DashboardService.DeleteProductAsync(productId);
            return Ok("Product deleted successfully");
        }


        [HttpPost("add-brand")]
        public async Task<IActionResult> AddBrand([FromBody] CreateBrandDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new
            {
                StatusCode = 401,
                ErrorMessage = "User not authenticated."
            });

            await serviceManager.DashboardService.AddBrandAsync(userId, dto);
            return Ok("Brand added successfully");
        }

        [HttpPut("update-brand/{brandId}")]
        public async Task<IActionResult> UpdateBrand(string brandId, [FromBody] UpdateBrandDto dto)
        {
            await serviceManager.DashboardService.UpdateBrandAsync(brandId, dto);
            return Ok("Brand updated successfully");
        }

        [HttpDelete("delete-brand/{brandId}")]
        public async Task<IActionResult> DeleteBrand(string brandId)
        {
            await serviceManager.DashboardService.DeleteBrandAsync(brandId);
            return Ok("Brand deleted successfully");
        }

    }
}
