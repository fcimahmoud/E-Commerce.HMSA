﻿
using Microsoft.AspNetCore.Http;

namespace Shared
{
    public class UpdateProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string BrandId { get; set; }
    }
}
