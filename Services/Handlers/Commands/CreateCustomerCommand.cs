using MediatR;
using Paessler.Task.Model.Models;

namespace Paessler.Task.Services.Handlers.Commands
{
    public class CreateCustomerCommand : IRequest<Customer>
    {
        public Customer Customer { get; set; }
    }
}