﻿
namespace Services.Abstractions
{
    public interface IOrderService
    {
        // Get Order By Id => OrderResult (Guid Id)
        public Task<OrderResult> GetOrderByIdAsync(Guid id);
        // Get Orders For User By Email => IEnumerable<OrderResult> (string email)
        public Task<IEnumerable<OrderResult>> GetOrdersByEmailAsync(string email);
        // Create Order => OrderRseult (OrderRequest , string email)
        public Task<OrderResult> CreateOrderAsync(OrderRequest orderRequest, string userEmail);
    }
}
