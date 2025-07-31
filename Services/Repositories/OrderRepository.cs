using Microsoft.EntityFrameworkCore;
using Paessler.Task.Model;
using Paessler.Task.Model.Models;
using Paessler.Task.Services.DTOs;
using Paessler.Task.Services.Repositories.IRepositories;

namespace Paessler.Task.Services.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly PostgresContext _context;

        public OrderRepository(PostgresContext context)
        {
            _context = context;
        }

        public async Task<Order> GetById(int id)
        {
            return await _context.Orders
                .Include(o => o.ProductOrdered).ThenInclude(po => po.Product)
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.id == id);
        }

        public Task<Order> CreateOrderAsync(OrderDTO order)
        {
            throw new NotImplementedException();
        }

        public Task<List<Order>> GetAll()
        {
            throw new NotImplementedException();
        }

    }
}