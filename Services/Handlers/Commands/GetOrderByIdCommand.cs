using MediatR;
using Paessler.Task.Services.DTOs;

namespace Paessler.Task.Services.Handlers.Commands
{
    public class GetOrderByIdCommand : IRequest<OrderDTO>
    {
        public int OrderId { get; set; }
    }
}