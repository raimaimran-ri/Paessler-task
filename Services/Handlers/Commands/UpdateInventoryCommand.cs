using MediatR;
using Paessler.Task.Model.Models;
using Paessler.Task.Services.DTOs;

namespace Paessler.Task.Services.Handlers.Commands
{
    public class UpdateProductInventoryCommand : IRequest<Product>
    {
        public int ProductId { get; set; }
        public int Amount { get; set; }
    }
}