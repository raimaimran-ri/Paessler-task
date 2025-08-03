using System.Net;
using System.Net.Http.Json;
using Paessler.Task.WebAPI;
using Paessler.Task.Services.DTOs;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;

public class OrderControllerIntegrationTests : IClassFixture<CustomWebAppFactory>
{
    private readonly HttpClient _client;
    private static OrderDTO orderDTO = new OrderDTO
    {
        InvoiceAddress = "123 Sample Street, 90402 Berlin",
        InvoiceEmailAddress = "customerabc@example.com",
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

    public OrderControllerIntegrationTests(CustomWebAppFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateOrder_ReturnsCreated_WhenValidOrder()
    {
        var response = await _client.PostAsJsonAsync("/api/order/create", orderDTO);
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task CreateOrder_ReturnsBadRequest_WhenValidationFails()
    {
        orderDTO.InvoiceEmailAddress = "invalidemail"; 
        var response = await _client.PostAsJsonAsync("/api/order/create", orderDTO);
        Console.WriteLine(await response.Content.ReadAsStringAsync());
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetOrderById_ReturnsOk_WhenOrderExists()
    {
        int orderId = 6;

        var response = await _client.GetAsync($"/api/order/{orderId}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetOrderById_Error_WhenOrderNotFound()
    {
        int orderId = 99999;

        var response = await _client.GetAsync($"/api/order/{orderId}");
        Assert.True(response.StatusCode == HttpStatusCode.InternalServerError);
    }
}
