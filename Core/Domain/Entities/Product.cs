
namespace Domain.Entities
{
    public class Product : BaseEntity<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public ProductBrand ProductBrand { get; set; } // Ref Navigational Prop
        public string BrandId {  get; set; }
    }
}
