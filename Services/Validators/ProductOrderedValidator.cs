using FluentValidation;
using Paessler.Task.Services.DTOs;
using Paessler.Task.Services.Repositories.IRepositories;

namespace Paessler.Task.Services.Validators
{
    public class ProductOrderedDTOValidator : AbstractValidator<ProductOrderedDTO>
    {
        public ProductOrderedDTOValidator(IProductRepository productRepo)
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product ID must not be empty.")
                .MustAsync(async (dto, amount, cancellation) =>
                {
                    var inventory = await productRepo.GetInventoryAmountAsync(dto.ProductId);
                    return amount <= inventory || inventory == 0;
                })
                .WithMessage("Product is out of stock.");
            RuleFor(x => x.ProductName).NotEmpty().WithMessage("Product name must not be empty.");
            RuleFor(x => x.ProductPrice).GreaterThan(0).WithMessage("Product price must be greater than zero.");
            RuleFor(x => x.ProductAmount).GreaterThan(0).WithMessage("Product amount must be greater than zero.");
        }
    }
}