using Paessler.Task.Model;
using Paessler.Task.Model.Models;
using Paessler.Task.Services.DTOs;

namespace Paessler.Task.Services.Repositories.IRepositories
{
    public interface IOrderRepository: IBaseRepository<Order>
    {
        /// <summary>
        /// This method takes an Order object and creates a new order.
        /// </summary>
        /// <returns>This method returns an Order object after creation.</returns>
        Task<Order> CreateOrderAsync(Order order);
    }
}
