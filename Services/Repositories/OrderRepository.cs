using AutoMapper;
using FluentValidation;
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
        private readonly IValidator<OrderDTO> _orderValidator;
        private readonly ILogger<OrderRepository> _logger;
        public OrderRepository(PostgresContext context, IMapper mapper, ICustomerRepository customerRepository, IProductRepository productRepository,
        IValidator<OrderDTO> orderValidator, ILogger<OrderRepository> logger)
        {
            _context = context;
            _mapper = mapper;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _orderValidator = orderValidator;
            _logger = logger;
        }

        public async Task<Order> GetById(int id)
        {
            return await _context.Orders
                .Include(o => o.ProductOrdered).ThenInclude(po => po.Product)
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.id == id);
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            try
            {
                order.created_at = DateTime.UtcNow;
                _context.Orders.Add(order);
                _logger.LogInformation("Order saved successfully with ID: {OrderId}", order.id);
                await _context.SaveChangesAsync();

                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order: {Message}", ex.Message);
                throw;
            }
        }

        public Task<List<Order>> GetAll()
        {
            throw new NotImplementedException();
        }

    }
}