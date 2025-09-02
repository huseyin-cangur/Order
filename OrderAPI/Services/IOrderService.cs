
using OrderAPI.Dtos;

namespace OrderAPI.Services
{
    public interface IOrderService
    {
        Task<CreateOrderDto> CreateOrder(CreateOrderDto orderDto);
        Task<List<OrderDto>> GetOrdersByCustomerId(Guid customerId);
        Task<OrderDto> GetById(Guid Id);
        Task<Guid>RemoveOrder(Guid Id); 
    }
}