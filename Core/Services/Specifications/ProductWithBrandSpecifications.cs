
namespace Services.Specifications
{
    internal class ProductWithBrandSpecifications : Specifications<Product>
    {
        // Use to retrieve Product by Id
        public ProductWithBrandSpecifications(string id)
            : base(product => product.Id == id)
        {
            AddInclude(product => product.ProductBrand);
        }

        // Use to Get All Products 
        public ProductWithBrandSpecifications(ProductSpecificationsParameters parameters)
            : base(product =>
                    (string.IsNullOrWhiteSpace(parameters.BrandId) || product.BrandId == parameters.BrandId) &&
                    (string.IsNullOrWhiteSpace(parameters.Search) || product.Name.ToLower().Contains(parameters.Search.ToLower().Trim())))
        {
            AddInclude(product => product.ProductBrand);

            ApplyPagination(parameters.PageIndex, parameters.PageSize);

            if (parameters.Sort is not null)
            {
                switch(parameters.Sort)
                {
                    case ProductSortOptions.PriceDesc:
                        SetOrderByDescending(p => p.Price);
                        break;
                    case ProductSortOptions.PriceAsc:
                        SetOrderBy(p => p.Price);
                        break;
                    case ProductSortOptions.NameDesc:
                        SetOrderByDescending(p => p.Name);
                        break;
                    default:
                        SetOrderBy(p => p.Name);
                        break;
                }
            }
        }
    }
}
