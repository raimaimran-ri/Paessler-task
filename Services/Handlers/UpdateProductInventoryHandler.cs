
using Paessler.Task.Services.Handlers.Commands;
using Paessler.Task.Services.DTOs;
using Paessler.Task.Services.Repositories.IRepositories;
using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.DataProtection;
using Paessler.Task.Model.Models;

namespace Paessler.Task.Services.Handlers
{
    public class UpdateProductInventoryHandler : IRequestHandler<UpdateProductInventoryCommand, Product>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateProductInventoryHandler> _logger;
        public UpdateProductInventoryHandler(IProductRepository repository, IMapper mapper, ILogger<UpdateProductInventoryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Product> Handle(UpdateProductInventoryCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetById(request.ProductId);
            if (product == null)
            {
                _logger.LogError("Product not found: {ProductId}", request.ProductId);
                return null;
            }
            if(product.inventory_amount < request.Amount)
            {
                _logger.LogError("Product is out of stock {ProductId}, {Amount}.", request.ProductId, request.Amount);
                throw new Exception("Product is out of stock");
            }
            product.inventory_amount -= request.Amount;

            var updatedProduct = await _repository.UpdateProductInventory(product);
            if (updatedProduct == null)
            {
                _logger.LogError("Failed to update inventory for Product ID: {ProductId}", request.ProductId);
                throw new Exception("Failed to update inventory");
            }

            _logger.LogInformation("Inventory updated for Product ID: {ProductId}", updatedProduct.id);
            return updatedProduct;
        }
    }
}
