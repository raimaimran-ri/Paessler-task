
using Paessler.Task.Services.Handlers.Commands;
using Paessler.Task.Services.DTOs;
using Paessler.Task.Services.Repositories.IRepositories;
using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.DataProtection;

namespace Paessler.Task.Services.Handlers
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdCommand, OrderDTO>
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;
        private readonly IDataProtector _dataProtector;
        private readonly ILogger<GetOrderByIdQueryHandler> _logger;
        public GetOrderByIdQueryHandler(IOrderRepository repository, IMapper mapper, IDataProtectionProvider dataProtectionProvider, ILogger<GetOrderByIdQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _dataProtector = dataProtectionProvider.CreateProtector("Customer");
            _logger = logger;
        }

        public async Task<OrderDTO> Handle(GetOrderByIdCommand request, CancellationToken cancellationToken)
        {
            var order = await _repository.GetById(request.OrderId);
            if (order == null)
            {
                _logger.LogError("Order not found: {OrderId}", request.OrderId);
                return null;
            }

            var orderDto = _mapper.Map<OrderDTO>(order);
            orderDto.InvoiceCreditCardNumber = _dataProtector.Unprotect(orderDto.InvoiceCreditCardNumber);
            return orderDto;
        }
    }
}
