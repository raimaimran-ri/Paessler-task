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

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            try
            {
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                return customer;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating customer: {Message}", ex.Message);
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

        public Task<Customer> UpdateCustomerAsync(Customer customer)
        {
            try
            {
                _context.Customers.Update(customer);
                return _context.SaveChangesAsync().ContinueWith(t => customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating customer: {Message}", ex.Message);
                throw;
            }
        }
    }
}