using CusstomerManagement.Application.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

namespace CustomerManagement.Presentation.Tests.Controllers;

public class CustomerControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly string baseUrl = "/api/customer";

    public CustomerControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Post_ValidCustomer_ReturnsCreated()
    {
        // Arrange
        var newCustomer = new AddCustomerDto
        {
            Id = Guid.NewGuid(),
            FirstName = "Test",
            LastName = "User",
            Email = $"test{Guid.NewGuid()}@mail.com"
        };

        // Act
        var response = await _client.PostAsJsonAsync(baseUrl, newCustomer);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Get_InvalidId_ReturnsNotFound()
    {
        // Arrange
        var fakeId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"{baseUrl}/{fakeId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        Assert.Contains("Resource not found", body?["message"]);
    }

    [Fact]
    public async Task Delete_InvalidId_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var response = await _client.DeleteAsync($"{baseUrl}/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Put_WithMismatchedId_ReturnsBadRequest()
    {
        // Arrange
        var id = Guid.NewGuid();
        var payload = new UpdateCustomerDto
        {
            Id = Guid.NewGuid(), // mismatch ID
            FirstName = "Mismatch",
            LastName = "ID"
        };

        // Act
        var response = await _client.PutAsJsonAsync($"{baseUrl}/{id}", payload);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}