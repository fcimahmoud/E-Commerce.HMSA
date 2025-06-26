
using Microsoft.AspNetCore.Http;

namespace Shared
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public IFormFile Image { get; set; } = default!;
        public decimal Price { get; set; }
        public string BrandId { get; set; }
    }
}
