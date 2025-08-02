using FluentValidation;
using Paessler.Task.Services.DTOs;
using Paessler.Task.Services.Repositories.IRepositories;

namespace Paessler.Task.Services.Validators
{
    public class CustomerDTOValidator : AbstractValidator<CustomerDTO>
    {
        public CustomerDTOValidator()
        {
            RuleFor(x => x.InvoiceEmailAddress)
                .NotEmpty()
                .Matches(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$")
                .WithMessage("Email address is invalid.");
            RuleFor(x => x.InvoiceCreditCardNumber).NotEmpty();
            RuleFor(x => x.InvoiceAddress).NotEmpty();
        }
    }
}