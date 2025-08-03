
using Paessler.Task.Services.Handlers.Commands;
using Paessler.Task.Services.DTOs;
using Paessler.Task.Services.Repositories.IRepositories;
using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.DataProtection;
using Paessler.Task.Model.Models;
using FluentValidation;

namespace Paessler.Task.Services.Handlers
{
    public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, Customer>
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;
        private readonly IDataProtector _dataProtector;
        private readonly ILogger<CreateCustomerHandler> _logger;
        private readonly IValidator<CustomerDTO> _customerValidator;
        private readonly bool _skipProtection;
        public CreateCustomerHandler(ICustomerRepository repository, IMapper mapper, IDataProtectionProvider dataProtectionProvider, ILogger<CreateCustomerHandler> logger, IValidator<CustomerDTO> customerValidator, bool skipProtection = false)
        {
            _repository = repository;
            _mapper = mapper;
            _dataProtector = dataProtectionProvider.CreateProtector("Customer");
            _logger = logger;
            _customerValidator = customerValidator;
            _skipProtection = skipProtection;
        }

        public async Task<Customer> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = request.Customer;
            var result = await _customerValidator.ValidateAsync(_mapper.Map<CustomerDTO>(customer));
            if (!result.IsValid)
            {
                _logger.LogError("Customer validation failed: {Errors}", result.Errors);
                throw new ValidationException(result.Errors);
            }

            var existingCustomer = await _repository.GetByEmailAndAddressAsync(customer.email, customer.address);
            if (existingCustomer == null)
            {
                customer.credit_card_number = _skipProtection ? customer.credit_card_number : _dataProtector.Protect(customer.credit_card_number);
                var customerCreated = await _repository.CreateCustomerAsync(customer);
                _logger.LogInformation("Customer created with ID: {CustomerId}", customerCreated.id);
                return customerCreated;
            }
            else
            {
                existingCustomer.credit_card_number = _skipProtection ? existingCustomer.credit_card_number : _dataProtector.Protect(customer.credit_card_number);
                var updatedCustomer = await _repository.UpdateCustomerAsync(existingCustomer);
                _logger.LogInformation("Customer updated with ID: {CustomerId}", updatedCustomer.id);
                return updatedCustomer;
            }
        }

    }
}
