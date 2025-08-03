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

public class CreateOrderHandlerTests
{
    private readonly Mock<IOrderRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateOrderHandler _handler;
    private readonly Mock<ILogger<CreateOrderHandler>> _logger;
    private readonly Mock<IValidator<OrderDTO>> _validatorMock;
    private readonly Mock<IMediator> _mediator;
    private readonly OrderDTO orderDTO = new OrderDTO
    {
        InvoiceAddress = "123 Sample Street, 90402 Berlin",
        InvoiceEmailAddress = "customer12334@example.com",
        InvoiceCreditCardNumber = "1234-5678-9101-1121",
        ProductOrdered = new List<ProductOrderedDTO>
            {
                new ProductOrderedDTO
                {
                    ProductId = 1,
                    ProductName = "Gaming Laptop",
                    ProductPrice = 1499.99f,
                    ProductAmount = 1
                },
                new ProductOrderedDTO
                {
                    ProductId = 2,
                    ProductName = "Gaming Headphones",
                    ProductPrice = 149.99f,
                    ProductAmount = 2
                }
            }
    };
    private readonly Order orderEntity = new Order
    {
        Customer = new Customer(),
        ProductOrdered = new List<ProductOrdered>
        {
            new ProductOrdered
            {
                id = 2,
                order_id = 1,
                product_id = 1,
                amount = 1,
                total_price = 1499.99f,
                Product = new Product
                {
                    id = 1,
                    name = "Gaming Laptop",
                    price = 1499.99f,
                    inventory_amount = 10
                }
            }
        }
    };
    private readonly OrderDTO createdOrderEntity = new OrderDTO { OrderNumber = 1 };
    private readonly Customer customerEntity = new Customer { id = 5 };
    private readonly Product updatedProduct = new Product { id = 1, price = 1499.99f, inventory_amount = 10 };

    public CreateOrderHandlerTests()
    {
        _repositoryMock = new Mock<IOrderRepository>();
        _mapperMock = new Mock<IMapper>();
        _logger = new Mock<ILogger<CreateOrderHandler>>();
        _validatorMock = new Mock<IValidator<OrderDTO>>();
        _mediator = new Mock<IMediator>();
        _handler = new CreateOrderHandler(_repositoryMock.Object, _mapperMock.Object, _logger.Object, _validatorMock.Object, _mediator.Object);
    }

    [Fact]
    public async Task Handle_Should_CreateOrderSuccessfully()
    {
        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<OrderDTO>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mapperMock.Setup(m => m.Map<Order>(orderDTO))
                   .Returns(orderEntity);
        _mediator.Setup(m => m.Send(It.IsAny<CreateCustomerCommand>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(customerEntity);
        _mediator.Setup(m => m.Send(It.IsAny<UpdateProductInventoryCommand>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(updatedProduct);
        _repositoryMock.Setup(r => r.CreateOrderAsync(It.IsAny<Order>()))
                       .ReturnsAsync(orderEntity);
        _mapperMock.Setup(m => m.Map<OrderDTO>(orderEntity))
                        .Returns(orderDTO);

        var result = await _handler.Handle(new CreateOrderCommand { Order = orderDTO }, CancellationToken.None);
        Assert.NotNull(result);

        _validatorMock.Verify(v => v.ValidateAsync(orderDTO, It.IsAny<CancellationToken>()), Times.Once);
        _mediator.Verify(m => m.Send(It.IsAny<CreateCustomerCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        _mediator.Verify(m => m.Send(It.IsAny<UpdateProductInventoryCommand>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        _repositoryMock.Verify(r => r.CreateOrderAsync(It.IsAny<Order>()), Times.Once);
        _mapperMock.Verify(m => m.Map<OrderDTO>(orderEntity), Times.Once);

    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenValidationFails()
    {
        var validationErrors = new List<FluentValidation.Results.ValidationFailure>
        {
            new FluentValidation.Results.ValidationFailure("InvoiceEmailAddress", "Invalid email")
        };

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<OrderDTO>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new FluentValidation.Results.ValidationResult(validationErrors));

        orderDTO.InvoiceEmailAddress = "invalid-email";
        await Assert.ThrowsAsync<FluentValidation.ValidationException>(() =>
            _handler.Handle(new CreateOrderCommand { Order = orderDTO }, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenCustomerCreationFails()
    {
        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<OrderDTO>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mapperMock.Setup(m => m.Map<Order>(orderDTO))
                   .Returns(orderEntity);
        _mediator.Setup(m => m.Send(It.IsAny<CreateCustomerCommand>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync((Customer)null);

        await Assert.ThrowsAsync<Exception>(() =>
            _handler.Handle(new CreateOrderCommand { Order = orderDTO }, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_Should_SkipInventoryUpdate_WhenProductIsNull()
    {
        var orderWithNullProduct = new Order
        {
            Customer = new Customer(),
            ProductOrdered = new List<ProductOrdered>
            {
                new ProductOrdered { Product = null, amount = 1 }
            }
        };

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<OrderDTO>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _mapperMock.Setup(m => m.Map<Order>(orderDTO))
                    .Returns(orderWithNullProduct);
        _mediator.Setup(m => m.Send(It.IsAny<CreateCustomerCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(customerEntity);
        _repositoryMock.Setup(r => r.CreateOrderAsync(It.IsAny<Order>()))
                    .ReturnsAsync(orderEntity);
        _mapperMock.Setup(m => m.Map<OrderDTO>(orderEntity))
                    .Returns(orderDTO);

        var result = await _handler.Handle(new CreateOrderCommand { Order = orderDTO }, CancellationToken.None);

        Assert.NotNull(result);
        _mediator.Verify(m => m.Send(It.IsAny<UpdateProductInventoryCommand>(), It.IsAny<CancellationToken>()), Times.Never);
    }

}
