
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Paessler.Task.Services.DTOs;
using Paessler.Task.Services.Handlers.Commands;
using Paessler.Task.WebAPI.Controllers;
using Microsoft.Extensions.Logging;
namespace Paessler.Task.Tests;

public class OrderControllerTests
{
    private readonly OrderController _controller;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ILogger<OrderController> _logger;
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

    public OrderControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _logger = new Mock<ILogger<OrderController>>().Object;
        _controller = new OrderController(_mediatorMock.Object, _logger);
    }
    [Fact]
    public async void Should_Create_Order()
    {
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateOrderCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new OrderDTO { OrderNumber = 1 });

        var result = await _controller.CreateOrder(orderDTO) as CreatedAtActionResult;
        Assert.NotNull(result.Value);
        Assert.Equal(201, result.StatusCode);

        var value = result.Value;
        var idProperty = value.GetType().GetProperty("id");
        Assert.NotNull(idProperty);
        var idValue = idProperty.GetValue(value);
        Assert.Equal(1, idValue);
    }

    [Fact]
    public async void Should_Get_Order_By_Id()
    {
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetOrderByIdCommand>(), default))
                     .ReturnsAsync(orderDTO);

        var result = await _controller.GetOrderById(1);

        var okResult = result.Result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(orderDTO, okResult.Value);
    }

    [Fact]
    public async void Should_Return_BadRequest_On_Validation_Error()
    {
        orderDTO.InvoiceEmailAddress = "invalidemailABC";
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateOrderCommand>(), It.IsAny<CancellationToken>()))
                     .Throws(new FluentValidation.ValidationException("Email address is invalid."));

        var result = await _controller.CreateOrder(orderDTO);
        var badRequestResult = result as BadRequestObjectResult;
        Assert.NotNull(badRequestResult);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public void Should_Return_BadRequest_If_Product_Out_of_Stock()
    {
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateOrderCommand>(), It.IsAny<CancellationToken>()))
                     .Throws(new FluentValidation.ValidationException("Product is out of stock"));

        var result = _controller.CreateOrder(orderDTO).Result;
        var badRequestResult = result as BadRequestObjectResult;
        Assert.NotNull(badRequestResult);
        Assert.Equal(400, badRequestResult.StatusCode);
    }
}
