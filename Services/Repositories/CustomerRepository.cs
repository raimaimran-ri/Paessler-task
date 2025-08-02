using Microsoft.EntityFrameworkCore;
using Paessler.Task.Model;
using Paessler.Task.Model.Models;
using Paessler.Task.Services.DTOs;
using Paessler.Task.Services.Repositories.IRepositories;
using Microsoft.AspNetCore.DataProtection;
using FluentValidation;

namespace Paessler.Task.Services.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly PostgresContext _context;
        private readonly IDataProtector _dataProtector;
        private readonly IValidator<CustomerDTO> _customerValidator;
        private readonly ILogger<CustomerRepository> _logger;

        public CustomerRepository(PostgresContext context, IDataProtectionProvider dataProtectionProvider, IValidator<CustomerDTO> customerValidator, ILogger<CustomerRepository> logger)
        {
            _context = context;
            _dataProtector = dataProtectionProvider.CreateProtector("Customer");
            _customerValidator = customerValidator;
            _logger = logger;
        }

        public async Task<Customer> CreateOrUpdateCustomerAsync(CustomerDTO customer)
        {
            try
            {
                var result = await _customerValidator.ValidateAsync(customer);
                if (!result.IsValid)
                {
                    _logger.LogError("Customer validation failed: {Errors}", result.Errors);
                    throw new ValidationException(result.Errors);
                }

                var existingCustomer = await GetByEmailAndAddressAsync(customer.InvoiceEmailAddress, customer.InvoiceAddress);
                if (existingCustomer == null)
                {
                    var customerEntity = new Customer
                    {
                        email = customer.InvoiceEmailAddress,
                        address = customer.InvoiceAddress,
                        credit_card_number = customer.InvoiceCreditCardNumber
                    };
                    _context.Customers.Add(customerEntity);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("New customer created with ID: {CustomerId}", customerEntity.id);
                    return customerEntity;
                }
                else
                {
                    var encryptedCardNumber = _dataProtector.Protect(customer.InvoiceCreditCardNumber);
                    existingCustomer.credit_card_number = encryptedCardNumber;
                    _context.Customers.Update(existingCustomer);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Customer updated with ID: {CustomerId}", existingCustomer.id);
                    return existingCustomer;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating or updating customer: {Message}", ex.Message);
                throw;
            }

        }

        public Task<List<Customer>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Customer> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Customer?> GetByEmailAndAddressAsync(string email, string address)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.email == email && c.address == address);
        }

    }
}