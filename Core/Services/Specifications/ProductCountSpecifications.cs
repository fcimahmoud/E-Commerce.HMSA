
namespace Services.Specifications
{
    public class ProductCountSpecifications : Specifications<Product>
    {
        public ProductCountSpecifications(ProductSpecificationsParameters parameters)
            : base(product =>
                    (string.IsNullOrWhiteSpace(parameters.BrandId) || product.BrandId == parameters.BrandId) &&
                    (string.IsNullOrWhiteSpace(parameters.Search) || product.Name.ToLower().Contains(parameters.Search.ToLower().Trim())))
        {
        }
    }
}
