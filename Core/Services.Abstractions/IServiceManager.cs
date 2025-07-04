﻿
namespace Services.Abstractions
{
    public interface IServiceManager
    {
        public IProductService ProductService { get; }
        public IBasketService BasketService { get; }
        public IAuthenticationService AuthenticationService { get; }
        public IOrderService OrderService { get; }
        public ICacheService CacheService { get; }
        public IDashboardService DashboardService { get; }

    }
}
