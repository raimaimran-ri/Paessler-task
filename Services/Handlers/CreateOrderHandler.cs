
using Paessler.Task.Services.Handlers.Commands;
using Paessler.Task.Services.DTOs;
using Paessler.Task.Services.Repositories.IRepositories;
using MediatR;
using AutoMapper;
using Paessler.Task.Model.Models;
using FluentValidation;

namespace Paessler.Task.Services.Handlers
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, OrderDTO>
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateOrderHandler> _logger;
        private readonly IValidator<OrderDTO> _orderValidator;
        private readonly IMediator _mediator;
        public CreateOrderHandler(IOrderRepository repository, IMapper mapper, ILogger<CreateOrderHandler> logger, IValidator<OrderDTO> orderValidator, IMediator mediator)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _orderValidator = orderValidator;
            _mediator = mediator;
        }

        public async Task<OrderDTO> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _orderValidator.ValidateAsync(request.Order);
            if (!validationResult.IsValid)
            {
                _logger.LogError("Order validation failed: {Errors}", validationResult.Errors);
                throw new ValidationException(validationResult.Errors);
            }

            var orderEntity = _mapper.Map<Order>(request.Order);

            var customer = _mediator.Send(new CreateCustomerCommand { Customer = orderEntity.Customer }).Result;
            if (customer == null)
            {
                _logger.LogError("Customer creation/updation failed for order with ID: {OrderId}", orderEntity.id);
                throw new Exception("Customer creation failed");
            }
            orderEntity.Customer = customer;

            foreach (var productOrdered in orderEntity.ProductOrdered)
            {
                if (productOrdered.Product != null)
                {
                    productOrdered.Product = await _mediator.Send(new UpdateProductInventoryCommand { ProductId = productOrdered.Product.id, Amount = productOrdered.amount });
                    productOrdered.total_price = productOrdered.Product.price * productOrdered.amount;
                }
            }

            var returnResult = await _repository.CreateOrderAsync(order: orderEntity);
            _logger.LogInformation("Order created with ID: {OrderId}", returnResult.id);
            return _mapper.Map<OrderDTO>(returnResult);
        }
    }
}
