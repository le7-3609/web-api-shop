using Entities;

namespace Repositories
{
    public interface IOrderRepository
    {
        Task<Order> GetByIdAsync(int id);
        Task<Order> AddOrderAsync(Order order);
    }
}