
namespace Services
{
    public sealed class ServiceManager (
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IOptions<JwtOptions> options, 
        UserManager<User> userManager, 
        ICacheRepository cacheRepository,
        IBasketRepository basketRepository,
         IEmailService emailService,
         IFileService fileService,
         IHttpContextAccessor httpContextAccessor
        )
        : IServiceManager
    {
        private readonly Lazy<IProductService> _productService
            = new (() => new ProductService(unitOfWork, mapper));

        private readonly Lazy<IBasketService> _lazyBasketService
            = new (() => new BasketService(basketRepository, mapper));

        private readonly Lazy<IAuthenticationService> _lazyAuthentication
            = new (() => new AuthenticationService(userManager, options, mapper, emailService));

        private readonly Lazy<IOrderService> _lazyOrdersService
            = new (() => new OrderService(unitOfWork, mapper, basketRepository));


        private readonly Lazy<ICacheService> _lazyCacheService
            = new(() => new CacheService(cacheRepository));


        private readonly Lazy<IDashboardService> _lazyDashboardService
            = new(() => new DashboardService(unitOfWork, fileService, httpContextAccessor));

        public IProductService ProductService => _productService.Value;
        public IBasketService BasketService => _lazyBasketService.Value;
        public IAuthenticationService AuthenticationService => _lazyAuthentication.Value;
        public IOrderService OrderService => _lazyOrdersService.Value;
        public ICacheService CacheService => _lazyCacheService.Value;

        public IDashboardService DashboardService => _lazyDashboardService.Value;
    }
}
