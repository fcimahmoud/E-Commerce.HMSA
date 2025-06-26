
namespace Shared.BasketModels
{
    public class BasketResultDto
    {
        public string Id { get; set; }
        public IEnumerable<BasketItemDTO> Items { get; set; }
        public decimal? ShippingPrice { get; set; }
    }
}
