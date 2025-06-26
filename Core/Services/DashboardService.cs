

using Domain.Contracts;

namespace Services
{
    public class DashboardService(
        IUnitOfWork unitOfWork,
        IFileService fileService,
        IHttpContextAccessor _httpContextAccessor
        )
        : IDashboardService
    {
        public async Task AddProductAsync(string userId, CreateProductDto dto)
        {
            var productRepo = unitOfWork.GetRepository<Product, string>();
            string imagePath = await fileService.SaveFileAsync(dto.Image, "images/products");

            var product = new Product
            {
                Id = Guid.NewGuid().ToString(),
                Name = dto.Name,
                PictureUrl = imagePath,
                Description = dto.Description,
                Price = dto.Price,
                BrandId = dto.BrandId,
            };

            await productRepo.AddAsync(product);
            await unitOfWork.SaveChangesAsync();
        }
        private string GetFullImageUrl(string imagePath)
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null) return imagePath;

            var baseUrl = $"{request.Scheme}://{request.Host}";
            return $"{baseUrl}/{imagePath}";
        }
        public async Task DeleteProductAsync(string productId)
        {
            var productRepo = unitOfWork.GetRepository<Product, string>();
            var product = await productRepo.GetAsync(productId);

            if (product == null)
                throw new Exception("Product not found");

            // Delete associated image if exists
            if (!string.IsNullOrEmpty(product.PictureUrl))
            {
                fileService.DeleteFile(product.PictureUrl);
            }

            productRepo.Delete(product);
            await unitOfWork.SaveChangesAsync();
        }
        public async Task UpdateProductAsync(string productId, UpdateProductDto dto)
        {
            var productRepo = unitOfWork.GetRepository<Product, string>();
            var product = await productRepo.GetAsync(productId);

            if (product == null)
                throw new Exception("Product not found");

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.BrandId = dto.BrandId;

            productRepo.Update(product);
            await unitOfWork.SaveChangesAsync();
        }
        public async Task UpdateProductImageAsync(string productId, IFormFile image)
        {
            var productRepo = unitOfWork.GetRepository<Product, string>();
            var product = await productRepo.GetAsync(productId);

            if (product == null)
                throw new Exception("Product not found");

            // Delete old image if exists
            if (!string.IsNullOrEmpty(product.PictureUrl))
            {
                fileService.DeleteFile(product.PictureUrl);
            }

            string imagePath = await fileService.SaveFileAsync(image, "images/products");
            product.PictureUrl = imagePath;

            productRepo.Update(product);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task AddBrandAsync(string userId, CreateBrandDto dto)
        {
            var brandRepo = unitOfWork.GetRepository<ProductBrand, string>();
            var brand = new ProductBrand
            {
                Id = Guid.NewGuid().ToString(),
                Name = dto.Name
            };

            await brandRepo.AddAsync(brand);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateBrandAsync(string brandId, UpdateBrandDto dto)
        {
            var brandRepo = unitOfWork.GetRepository<ProductBrand, string>();
            var brand = await brandRepo.GetAsync(brandId);

            if (brand == null)
                throw new Exception("Brand not found");

            brand.Name = dto.Name;

            brandRepo.Update(brand);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteBrandAsync(string brandId)
        {
            var brandRepo = unitOfWork.GetRepository<ProductBrand, string>();
            var brand = await brandRepo.GetAsync(brandId);

            if (brand == null)
                throw new Exception("Brand not found");

            brandRepo.Delete(brand);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
