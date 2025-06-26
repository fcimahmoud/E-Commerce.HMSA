
namespace Presentation
{
    public class ProductsController(IServiceManager serviceManager) : ApiController
    {
        [RedisCache]
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<ProductResultDTO>>> GetAllProducts
            ([FromQuery] ProductSpecificationsParameters parameters)
        {
            var products = await serviceManager.ProductService.GetAllProductsAsync(parameters);
            return Ok(products);
        }
        [HttpGet("Brands")]
        public async Task<ActionResult<IEnumerable<BrandResultDTO>>> GetAllBrands()
        {
            var brands = await serviceManager.ProductService.GetAllBrandsAsync();
            return Ok(brands);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<BrandResultDTO>>> GetProduct(string id)
        {
            var product = await serviceManager.ProductService.GetProductByIdAsync(id);
            return Ok(product);
        }
        [HttpGet("Brands/{id}")]
        public async Task<ActionResult<IEnumerable<BrandResultDTO>>> GetBrandById(string id)
        {
            var brand = await serviceManager.ProductService.GetBrandByIdAsync(id);
            return Ok(brand);
        }
    }
}
