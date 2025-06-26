﻿
namespace Presentation
{
    [Authorize]
    public class OrdersController (IServiceManager serviceManager)
        : ApiController
    {
        [HttpPost]
        public async Task<ActionResult<OrderResult>> Create(OrderRequest orderRequest)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var order = await serviceManager.OrderService.CreateOrderAsync(orderRequest, email);
            return Ok(order);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResult>>> GetOrders()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var orders = await serviceManager.OrderService.GetOrdersByEmailAsync(email);
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResult>> GetOrder(Guid id)
        {
            var order = await serviceManager.OrderService.GetOrderByIdAsync(id);
            return Ok(order);
        }

    }
}
