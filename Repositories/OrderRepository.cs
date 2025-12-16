using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class OrderRepository : IOrderRepository
    {
        MyShopContext _myShopContext;

        public OrderRepository(MyShopContext shopContext)
        {
            _myShopContext = shopContext;
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await _myShopContext.Orders.FirstOrDefaultAsync(order => order.OrderId == id);
        }

        public async Task<Order> AddOrderAsync(Order order)
        {
            await _myShopContext.Orders.AddAsync(order);
            await _myShopContext.SaveChangesAsync();
            return order;
        }
    }
}