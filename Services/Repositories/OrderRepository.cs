using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;

        public OrderRepository(PostgresContext context, IMapper mapper, ICustomerRepository customerRepository, IProductRepository productRepository)
        {
            _context = context;
            _mapper = mapper;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
        }

        public async Task<Order> GetById(int id)
        {
            return await _context.Orders
                .Include(o => o.ProductOrdered).ThenInclude(po => po.Product)
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.id == id);
        }

        public async Task<Order> CreateOrderAsync(OrderDTO order)
        {
            var orderEntity = _mapper.Map<Order>(order);
            var customerDto = _mapper.Map<CustomerDTO>(orderEntity.Customer);
            var customer = await _customerRepository.CreateOrUpdateCustomerAsync(customerDto);
            orderEntity.Customer = customer;

            foreach (var productOrdered in orderEntity.ProductOrdered)
            {
                if (productOrdered.Product != null)
                {
                    productOrdered.Product = await _productRepository.UpdateProductInventory(productOrdered.Product.id, productOrdered.amount);
                    productOrdered.total_price = productOrdered.Product.price * productOrdered.amount;
                }
            }
            _context.Orders.Add(orderEntity);
            await _context.SaveChangesAsync();

            return orderEntity;
        }

        public Task<List<Order>> GetAll()
        {
            throw new NotImplementedException();
        }

    }
}