using Microsoft.EntityFrameworkCore;
using Paessler.Task.Model;
using Paessler.Task.Model.Models;
using Paessler.Task.Services.DTOs;
using Paessler.Task.Services.Repositories.IRepositories;
using Microsoft.AspNetCore.DataProtection;

namespace Paessler.Task.Services.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly PostgresContext _context;
        private readonly IDataProtector _dataProtector;

        public CustomerRepository(PostgresContext context, IDataProtectionProvider dataProtectionProvider)
        {
            _context = context;
            _dataProtector = dataProtectionProvider.CreateProtector("Customer");
        }

        public async Task<Customer> CreateOrUpdateCustomerAsync(CustomerDTO customer)
        {
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
                return customerEntity;
            }
            else
            {
                var encryptedCardNumber = _dataProtector.Protect(customer.InvoiceCreditCardNumber);
                existingCustomer.credit_card_number = encryptedCardNumber;
                _context.Customers.Update(existingCustomer);
                await _context.SaveChangesAsync();
                return existingCustomer;
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