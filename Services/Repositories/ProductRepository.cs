using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Paessler.Task.Model;
using Paessler.Task.Model.Models;
using Paessler.Task.Services.DTOs;
using Paessler.Task.Services.Repositories.IRepositories;

namespace Paessler.Task.Services.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly PostgresContext _context;

        public ProductRepository(PostgresContext context)
        {
            _context = context;
        }

        public Task<List<Product>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> UpdateProductInventory(int productId, int amount)
        {
            var product = _context.Products.Find(productId);
            if (product == null) 
            {
                throw new KeyNotFoundException($"Product with ID {productId} not found.");
            }
            product.inventory_amount -= amount;
            await _context.SaveChangesAsync();
            return product;
        }
    }
}