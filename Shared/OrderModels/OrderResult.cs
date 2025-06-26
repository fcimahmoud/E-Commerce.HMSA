
namespace Shared.OrderModels
{
    public record OrderResult
    {
        public Guid Id { get; set; }
        public string UserEmail { get; set; }
        public AddressDTO ShippingAddress { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public ICollection<OrderItemDTO> OrderItems { get; set; } = new List<OrderItemDTO>();
        public decimal SubTotal { get; set; } = 0;
        public decimal Total { get; set; } = 0;
    }
}
