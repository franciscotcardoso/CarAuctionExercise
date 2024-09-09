using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using CarAuctionExercise.Application.DTOs.Vehicles;
using CarAuctionExercise.Domain;
using CarAuctionExercise.Infrastructure.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CarAuctionExercise.Api.IntegrationTests.Controllers;

public class VehiclesControllerTests: IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public VehiclesControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        
        IServiceRepository<Vehicle> vehicleRepository = _factory.Services.GetRequiredService<IServiceRepository<Vehicle>>();
        
        vehicleRepository.DeleteAll();
    }
    
    [Fact]
    public async Task Post_Vehicle_ReturnsCreatedStatusCode()
    {
        // Arrange
        var client = _factory.CreateClient();
        var licensePlate = "licensePlate1";
        var request = Fixtures.AddVehicleFixture.GetAddVehicle(licensePlate: licensePlate);

        // Act
        var response = await client.PostAsJsonAsync("api/v1/vehicles", request);
        var content = await response.Content.ReadFromJsonAsync<AvailableVehicle>(GetJsonSerializerOptions());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        content!.LicensePlate.Should().Be("licensePlate1");
    }
    
    [Fact]
    public async Task SearchForAddedVehicle_ByManufacturer_ReturnsSuccessStatusCode()
    {
        // Arrange
        var client = _factory.CreateClient();
        var licensePlate = "licensePlate1";
        var manufacturer = "manufacturer1";
        
        // Act
        var request = Fixtures.AddVehicleFixture.GetAddVehicle(licensePlate: licensePlate, manufacturer: manufacturer);
        await client.PostAsJsonAsync("api/v1/vehicles", request);
        
        var response = await client.GetAsync($"api/v1/vehicles/search/?manufacturer={request.Manufacturer}");
        var content = await response.Content.ReadFromJsonAsync<IEnumerable<AvailableVehicle>>(GetJsonSerializerOptions());

        // Assert
        response.EnsureSuccessStatusCode();
        content!.First().LicensePlate.Should().Be(licensePlate);
        content!.First().Manufacturer.Should().Be(manufacturer);
    }
    
    [Fact]
    public async Task SearchForAddedVehicle_ByModel_ReturnsSuccessStatusCode()
    {
        // Arrange
        var client = _factory.CreateClient();
        var licensePlate = "licensePlate1";
        var model = "model1";
        
        // Act
        var request = Fixtures.AddVehicleFixture.GetAddVehicle(licensePlate: licensePlate, model: model);
        
        await client.PostAsJsonAsync("api/v1/vehicles", request);
        var response = await client.GetAsync($"api/v1/vehicles/search/?model={request.Model}");
        var content = await response.Content.ReadFromJsonAsync<IEnumerable<AvailableVehicle>>(GetJsonSerializerOptions());

        // Assert
        response.EnsureSuccessStatusCode();
        content!.First().LicensePlate.Should().Be(licensePlate);
        content!.First().Model.Should().Be(model);
    }
    
    [Fact]
    public async Task SearchForAddedVehicle_ByYear_ReturnsSuccessStatusCode()
    {
        // Arrange
        var client = _factory.CreateClient();
        var licensePlate = "licensePlate1";
        var year = 2010;
        
        // Act
        var request = Fixtures.AddVehicleFixture.GetAddVehicle(licensePlate: licensePlate, year: year);
        
        await client.PostAsJsonAsync("api/v1/vehicles", request);
        var response = await client.GetAsync($"api/v1/vehicles/search/?year={request.Year}");
        var content = await response.Content.ReadFromJsonAsync<IEnumerable<AvailableVehicle>>(GetJsonSerializerOptions());

        // Assert
        response.EnsureSuccessStatusCode();
        content!.First().LicensePlate.Should().Be(licensePlate);
        content!.First().Year.Should().Be(year);
    }
    
    [Fact]
    public async Task SearchForAddedVehicle_ByVehicleType_ReturnsSuccessStatusCode()
    {
        // Arrange
        var client = _factory.CreateClient();
        var licensePlate = "licensePlate1";
        var vehicleType = VehicleType.Hatchback;
        
        // Act
        var request = Fixtures.AddVehicleFixture.GetAddVehicle(licensePlate: licensePlate, vehicleType: vehicleType);
        
        await client.PostAsJsonAsync("api/v1/vehicles", request);
        var response = await client.GetAsync($"api/v1/vehicles/search/?vehicleType={request.VehicleType}");
        var content = await response.Content.ReadFromJsonAsync<IEnumerable<AvailableVehicle>>(GetJsonSerializerOptions());

        // Assert
        response.EnsureSuccessStatusCode();
        content!.First().LicensePlate.Should().Be(licensePlate);
        content!.First().Type.Should().Be(vehicleType);
    }
    private static JsonSerializerOptions GetJsonSerializerOptions()
    {
        var jsonOptions = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() },
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        return jsonOptions;
    }
    
}