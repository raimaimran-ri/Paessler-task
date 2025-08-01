
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
        public CreateOrderHandler(IOrderRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OrderDTO> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = _mapper.Map<Order>(request.Order);
            

            return _mapper.Map<OrderDTO>(new OrderDTO());
        }
    }
}
