namespace Shared.BasketModels
{
    public record BasketDTO
    {
        public IEnumerable<BasketItemDTO> Items { get; set; }
        public decimal? ShippingPrice { get; set; }
    }
}
