using FluentValidation;
using Paessler.Task.Services.DTOs;
using Paessler.Task.Services.Repositories.IRepositories;

namespace Paessler.Task.Services.Validators
{
    public class OrderDTOValidator : AbstractValidator<OrderDTO>
    {
        public OrderDTOValidator(IProductRepository productRepo)
        {
            RuleForEach(x => x.ProductOrdered).SetValidator(new ProductOrderedDTOValidator(productRepo));
        }
    }
}