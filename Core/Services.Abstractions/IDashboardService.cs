
namespace Services.Abstractions
{
    public interface IDashboardService
    {
        Task AddProductAsync(string userId, CreateProductDto dto);
        Task DeleteProductAsync(string productId);
        Task UpdateProductAsync(string productId, UpdateProductDto dto);
        Task UpdateProductImageAsync(string productId, IFormFile image);

        Task AddBrandAsync(string userId, CreateBrandDto dto);
        Task UpdateBrandAsync(string brandId, UpdateBrandDto dto);
        Task DeleteBrandAsync(string brandId);
    }
}
