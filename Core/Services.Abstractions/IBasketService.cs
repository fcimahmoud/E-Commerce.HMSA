
using Shared.BasketModels;

namespace Services.Abstractions
{
    public interface IBasketService
    {
        public Task<BasketResultDto?> GetBasketAsync(string id);
        public Task<BasketResultDto?> UpdateBasketAsync(BasketDTO basket);
        public Task<bool> DeleteBasketAsync(string id);
    }
}
