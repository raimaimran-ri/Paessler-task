using Paessler.Task.Model;
using Paessler.Task.Model.Models;
using Paessler.Task.Services.DTOs;

namespace Paessler.Task.Services.Repositories.IRepositories
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        /// <summary>
        /// This method takes a product and updates the inventory.
        /// </summary>
        /// <returns>This method returns an updated product after updation</returns>
        Task<Product> UpdateProductInventory(Product product);

        /// <summary>
        /// This method takes a product id and returns the current inventory amount.
        /// </summary>
        /// <returns>This method returns the current inventory amount</returns>
        Task<int> GetInventoryAmountAsync(int productId);
    }
}
