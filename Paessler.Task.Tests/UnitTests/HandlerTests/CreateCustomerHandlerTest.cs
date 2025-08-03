using System.ComponentModel.DataAnnotations;
using AutoMapper;
using FluentValidation;
using Paessler.Task.Model.Models;
using Paessler.Task.Services.DTOs;
using Paessler.Task.Services.Handlers;
using Paessler.Task.Services.Handlers.Commands;
using Paessler.Task.Services.Repositories.IRepositories;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.DataProtection;
using Moq;

public class CreateCustomerHandlerTests
{
    private readonly Mock<ICustomerRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateCustomerHandler _handler;
    private readonly Mock<ILogger<CreateCustomerHandler>> _logger;
    private readonly Mock<IValidator<CustomerDTO>> _validatorMock;
    private readonly Mock<IDataProtectionProvider> _dataProtectionProviderMock;
    private readonly CustomerDTO customerDTO = new CustomerDTO
    {
        InvoiceAddress = "123 Sample Street, 90402 Berlin",
        InvoiceEmailAddress = "customer12334@example.com",
        InvoiceCreditCardNumber = "1234-5678-9101-1121"
    };
    private readonly Customer customer = new Customer
    {
        id = 1,
        address = "123 Sample Street, 90402 Berlin",
        email = "customer@example.com",
        credit_card_number = "xyzprotectedstring"
    };

    public CreateCustomerHandlerTests()
    {
        _repositoryMock = new Mock<ICustomerRepository>();
        _mapperMock = new Mock<IMapper>();
        _logger = new Mock<ILogger<CreateCustomerHandler>>();
        _validatorMock = new Mock<IValidator<CustomerDTO>>();
        _dataProtectionProviderMock = new Mock<IDataProtectionProvider>();
        _dataProtectionProviderMock.Setup(dp => dp.CreateProtector(It.IsAny<string>()))
            .Returns(new Mock<IDataProtector>().Object);
        _handler = new CreateCustomerHandler(_repositoryMock.Object, _mapperMock.Object, dataProtectionProvider: _dataProtectionProviderMock.Object,
                                              logger: _logger.Object, customerValidator: _validatorMock.Object, true);
    }

    [Fact]
    public async Task Handle_Should_CreateCustomerSuccessfully_IfCustomerDoesntExist()
    {
        _mapperMock.Setup(m => m.Map<CustomerDTO>(It.IsAny<Customer>()))
                    .Returns(customerDTO);
        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<CustomerDTO>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _repositoryMock.Setup(r => r.GetByEmailAndAddressAsync(It.IsAny<string>(), It.IsAny<string>()))
                       .ReturnsAsync((Customer)null);
        _repositoryMock.Setup(r => r.CreateCustomerAsync(It.IsAny<Customer>()))
                       .ReturnsAsync(customer);

        var result = await _handler.Handle(new CreateCustomerCommand { Customer = customer }, CancellationToken.None);

        Assert.NotNull(result);

        _validatorMock.Verify(v => v.ValidateAsync(customerDTO, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.GetByEmailAndAddressAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        _repositoryMock.Verify(r => r.CreateCustomerAsync(It.IsAny<Customer>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_UpdateCustomerSuccessfully_IfCustomerExists()
    {
        _mapperMock.Setup(m => m.Map<CustomerDTO>(It.IsAny<Customer>()))
                    .Returns(customerDTO);
        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<CustomerDTO>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _repositoryMock.Setup(r => r.GetByEmailAndAddressAsync(It.IsAny<string>(), It.IsAny<string>()))
                       .ReturnsAsync(customer);
        _repositoryMock.Setup(r => r.UpdateCustomerAsync(It.IsAny<Customer>()))
                       .ReturnsAsync(customer);

        var result = await _handler.Handle(new CreateCustomerCommand { Customer = customer }, CancellationToken.None);
        Assert.NotNull(result);
        Assert.Equal(customer.id, result.id);

        _validatorMock.Verify(v => v.ValidateAsync(customerDTO, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.GetByEmailAndAddressAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        _repositoryMock.Verify(r => r.UpdateCustomerAsync(It.IsAny<Customer>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ThrowValidationException_WhenValidationFails()
    {
        _mapperMock.Setup(m => m.Map<CustomerDTO>(It.IsAny<Customer>()))
                    .Returns(customerDTO);
        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<CustomerDTO>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure>
                      {
                          new FluentValidation.Results.ValidationFailure("Email", "Email is required")
                      }));

        await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => _handler.Handle(new CreateCustomerCommand { Customer = customer }, CancellationToken.None));

        _validatorMock.Verify(v => v.ValidateAsync(customerDTO, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.GetByEmailAndAddressAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        _repositoryMock.Verify(r => r.CreateCustomerAsync(It.IsAny<Customer>()), Times.Never);
    }
}
