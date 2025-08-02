using Paessler.Task.Model;
using Paessler.Task.Model.Models;
using Paessler.Task.Services.DTOs;

namespace Paessler.Task.Services.Repositories.IRepositories
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
        /// <summary>
        /// This method takes an CustomerDTO object, matches the email and address if it already exists.
        /// If exists, it updates the existing customer with the new card details.
        /// If not, it creates a new customer with the provided details.
        /// </summary>
        /// <returns>This method returns an Customer object after creation.</returns>
        Task<Customer> CreateOrUpdateCustomerAsync(CustomerDTO customer);

        /// <summary>
        /// This method takes the customer address and email and returns the corresponding customer.
        /// If customer does not exist, it returns null.
        /// It is used to check if a customer already exists before creating a new one.
        /// </summary>
        /// <returns>This method returns an Customer object.</returns>
        Task<Customer?> GetByEmailAndAddressAsync(string email, string address);
    }
}
