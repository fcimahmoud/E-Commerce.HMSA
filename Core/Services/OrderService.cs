
namespace Services
{
    internal class OrderService (IUnitOfWork unitOfWork, IMapper mapper, 
        IBasketRepository basketRepository)
        : IOrderService
    {
        public async Task<OrderResult> CreateOrderAsync(OrderRequest orderRequest, string userEmail)
        {
            // 1. Address
            var address = mapper.Map<Address>(orderRequest.shipToAddress);

            // 2. Order Items :: Basket => Basket Items => Order Items
            var basket = await basketRepository.GetBasketAsync(orderRequest.BasketId)
                ?? throw new BasketNotFoundException(orderRequest.BasketId);

            var orderItems = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var product = await unitOfWork.GetRepository<Product, string>()
                    .GetAsync(item.Id) ?? throw new ProductNotFoundException(item.Id);
                orderItems.Add(CreateOrderItem(item, product));
            }

            // 4. SubTotal
            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);

            var orderRepo = unitOfWork.GetRepository<Order, Guid>();

            // Save To DataBase
            var order = new Order(userEmail, address, orderItems, subTotal);
            await orderRepo.AddAsync(order);
            await unitOfWork.SaveChangesAsync();

            // Map & Return
            return mapper.Map<OrderResult>(order);
        }
        private OrderItem CreateOrderItem(BasketItem item, Product product)
            => new OrderItem(new ProductInOrderItem(product.Id, product.Name, product.PictureUrl), item.Quantity, product.Price);

        public async Task<OrderResult> GetOrderByIdAsync(Guid id)
        {
            var order = await unitOfWork.GetRepository<Order, Guid>()
                .GetAsync(new OrderWithIncludeSpecifications(id))
                ?? throw new OrderNotFoundException(id);

            return mapper.Map<OrderResult>(order);
        }

        public async Task<IEnumerable<OrderResult>> GetOrdersByEmailAsync(string email)
        {
            var orders = await unitOfWork.GetRepository<Order, Guid>()
                .GetAllAsync(new OrderWithIncludeSpecifications(email));

            return mapper.Map<IEnumerable<OrderResult>>(orders);
        }
    }
}
