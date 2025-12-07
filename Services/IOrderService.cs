using Entities;

namespace Services
{
    public interface IOrderService
    {
        Task<Order> GetByIdAsync(int id);
        Task<Order> AddOrderAsync(Order order);
    }
}