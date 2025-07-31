namespace Paessler.Task.Services.Handlers.Commands
{
    public class CreateOrderCommand : IRequest<OrderDTO>
    {
        public OrderDTO Order { get; set; }
    }
}