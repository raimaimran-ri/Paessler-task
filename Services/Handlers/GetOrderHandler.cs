
using Paessler.Task.Services.Handlers.Commands;
using Paessler.Task.Services.DTOs;
using Paessler.Task.Services.Repositories.IRepositories;
using MediatR;
using AutoMapper;

namespace Paessler.Task.Services.Handlers
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdCommand, OrderDTO>
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;
        public GetOrderByIdQueryHandler(IOrderRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OrderDTO> Handle(GetOrderByIdCommand request, CancellationToken cancellationToken)
        {
            var order = await _repository.GetById(request.OrderId);
            if (order == null) return null;

            return _mapper.Map<OrderDTO>(order);
        }
    }
}
