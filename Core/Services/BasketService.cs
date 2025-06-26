
using Shared.BasketModels;

namespace Services
{
    internal class BasketService(IBasketRepository basketRepository, IMapper mapper)
        : IBasketService
    {
        public async Task<bool> DeleteBasketAsync(string id)
            => await basketRepository.DeleteBasketAsync(id);

        public async Task<BasketResultDto?> GetBasketAsync(string id)
        {
            var basket = await basketRepository.GetBasketAsync(id);
            return basket is null ? throw new BasketNotFoundException(id) 
                : mapper.Map<BasketResultDto?>(basket);
        }

        public async Task<BasketResultDto?> UpdateBasketAsync(BasketDTO basket)
        {
            var customerBasket = mapper.Map<CustomerBasket>(basket);
            customerBasket.Id = Guid.NewGuid().ToString();
            var updatedBasket = await basketRepository.UpdateBasketAsync(customerBasket);
            return updatedBasket is null ? throw new Exception("Can't Update Basket Now !!") : mapper.Map<BasketResultDto>(updatedBasket);
        }
    }
}
