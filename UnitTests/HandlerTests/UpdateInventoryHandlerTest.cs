using System.ComponentModel.DataAnnotations;
using AutoMapper;
using FluentValidation;
using MediatR;
using Moq;
using Paessler.Task.Model.Models;
using Paessler.Task.Services.DTOs;
using Paessler.Task.Services.Handlers;
using Paessler.Task.Services.Handlers.Commands;
using Paessler.Task.Services.Repositories.IRepositories;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.DataProtection;

public class UpdateProductInventoryHandlerTests
{
    private readonly Mock<IProductRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UpdateProductInventoryHandler _handler;
    private readonly Mock<ILogger<UpdateProductInventoryHandler>> _logger;
    private readonly UpdateProductInventoryCommand _command = new UpdateProductInventoryCommand
    {
        ProductId = 1,
        Amount = 5
    };

    public UpdateProductInventoryHandlerTests()
    {
        _repositoryMock = new Mock<IProductRepository>();
        _mapperMock = new Mock<IMapper>();
        _logger = new Mock<ILogger<UpdateProductInventoryHandler>>();
        _handler = new UpdateProductInventoryHandler(_repositoryMock.Object, _mapperMock.Object, _logger.Object);
    }


    [Fact]
    public async Task Handle_Should_ReturnUpdatedProduct_WhenProductExists()
    {
        _repositoryMock.Setup(r => r.GetById(_command.ProductId))
                      .ReturnsAsync(new Product { id = _command.ProductId, inventory_amount = _command.Amount });
        _repositoryMock.Setup(r => r.UpdateProductInventory(It.IsAny<Product>()))
                      .ReturnsAsync(new Product { id = _command.ProductId, inventory_amount = 0 });

        var result = await _handler.Handle(_command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(_command.ProductId, result.id);
        Assert.Equal(0, result.inventory_amount);
    }

    [Fact]
    public async Task Handle_Should_LogError_WhenProductNotFound()
    {
        _repositoryMock.Setup(r => r.GetById(_command.ProductId))
                      .ReturnsAsync((Product)null);

        var result = await _handler.Handle(_command, CancellationToken.None);

        Assert.Null(result);
    }

}
