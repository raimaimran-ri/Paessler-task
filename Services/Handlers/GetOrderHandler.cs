
using Paessler.Task.Services.Handlers.Commands;
using Paessler.Task.Services.DTOs;
using Paessler.Task.Services.Repositories.IRepositories;
using MediatR;

namespace Paessler.Task.Services.Handlers
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdCommand, OrderDTO>
    {
        private readonly IOrderRepository _repository;

        public GetOrderByIdQueryHandler(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<OrderDTO> Handle(GetOrderByIdCommand request, CancellationToken cancellationToken)
        {
            var order = await _repository.GetById(request.OrderId);
            if (order == null) return null;

            return new OrderDTO();
        }

    }
}
