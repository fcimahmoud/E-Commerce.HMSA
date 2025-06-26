
namespace Services
{
    internal class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
    {
        public async Task<IEnumerable<BrandResultDTO>> GetAllBrandsAsync()
        {
            // 1. Retrieve All Brands => UnitOfWork
            var brands = await unitOfWork.GetRepository<ProductBrand, string>().GetAllAsync();
            // 2. Map to BrandResultDTO => IMapper
            var brandsResult = mapper.Map<IEnumerable<BrandResultDTO>>(brands);
            // 3. Return
            return brandsResult;
        }

        public async Task<PaginatedResult<ProductResultDTO>> GetAllProductsAsync(ProductSpecificationsParameters parameters)
        {
            var products = await unitOfWork.GetRepository<Product, string>()
                .GetAllAsync(new ProductWithBrandSpecifications(parameters));
            var productsResult = mapper.Map<IEnumerable<ProductResultDTO>>(products);

            var count = productsResult.Count();
            var totalCount = await unitOfWork.GetRepository<Product, string>()
                .CountAsync(new ProductCountSpecifications(parameters));

            var result = new PaginatedResult<ProductResultDTO>
                (
                    parameters.PageIndex,
                    count,
                    totalCount,
                    productsResult
                );

            return result;
        }


        public async Task<ProductResultDTO> GetProductByIdAsync(string id)
        {
            var product = await unitOfWork.GetRepository<Product, string>()
                .GetAsync(new ProductWithBrandSpecifications(id));

            return product is null ? throw new ProductNotFoundException(id) 
                : mapper.Map<ProductResultDTO>(product);
        }
        public async Task<BrandResultDTO> GetBrandByIdAsync(string id)
        {
            var brand = await unitOfWork.GetRepository<ProductBrand, string>()
                .GetAsync(id);

            return brand is null ? throw new Exception($"Brand with id {id} not foud")
                : mapper.Map<BrandResultDTO>(brand);
        }
    }
}
