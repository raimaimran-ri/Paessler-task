
using Paessler.Task.Services.Handlers.Commands;
using Paessler.Task.Services.DTOs;
using Paessler.Task.Services.Repositories.IRepositories;
using MediatR;
using AutoMapper;
using Paessler.Task.Model.Models;

namespace Paessler.Task.Services.Handlers
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, OrderDTO>
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateOrderHandler> _logger;

        public CreateOrderHandler(IOrderRepository repository, IMapper mapper, ILogger<CreateOrderHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OrderDTO> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var returnResult = await _repository.CreateOrderAsync(order: request.Order);
            _logger.LogInformation("Order created with ID: {OrderId}", returnResult.id);
            return _mapper.Map<OrderDTO>(returnResult);
        }
    }
}
