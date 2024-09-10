namespace CarAuctionExercise.Application.UnitTests.Services;

using CarAuctionExercise.Application.DTOs.Vehicles;
using CarAuctionExercise.Application.Services;
using CarAuctionExercise.Application.Specifications.Vehicles;
using CarAuctionExercise.Domain;
using CarAuctionExercise.Infrastructure.Data;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class VehiclesManagementServiceTests
{
    [Theory]
    [InlineData(VehicleType.Sedan, 5, null, null)]
    [InlineData(VehicleType.Hatchback, 5, null, null)]
    [InlineData(VehicleType.SUV, null, 6, null)]
    [InlineData(VehicleType.Truck, null, null, 10000f)]
    public void AddVehicle_WithValidInformation_ShouldAddVehicle(
        VehicleType vehicleType,
        int? doorsNumber,
        int? seatsNumber,
        float? loadCapacity)
    {
        // Arrange
        const string manufacturer = "Manufacturer";
        const string model = "Model";
        const int year = 2022;
        const string licensePlate = "12-CD-34";

        var vehiclesRepository = new ServiceRepository<Vehicle>();
        var mockLogger = new Mock<ILogger<VehiclesManagementService>>();

        var vehiclesManagementService = new VehiclesManagementService(vehiclesRepository, mockLogger.Object);

        // Act
        var vehicle = new AddVehicle(
            manufacturer,
            model,
            year,
            vehicleType,
            licensePlate,
            doorsNumber,
            seatsNumber,
            loadCapacity);

        var result = vehiclesManagementService.Add(vehicle);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var addedVehicle = vehiclesRepository.Find(new FindVehicleByLicensePlateSpec(licensePlate));
        addedVehicle.Should().NotBeNull();
        addedVehicle!.Manufacturer.Should().Be(manufacturer);
        addedVehicle.Model.Should().Be(model);
        addedVehicle.Year.Should().Be(year);
        addedVehicle.Type.Should().Be(vehicleType);
        addedVehicle.LicensePlate.Should().Be(licensePlate);
    }

    [Theory]
    [InlineData(VehicleType.Sedan)]
    [InlineData(VehicleType.Hatchback)]
    public void AddVehicleWithDoorsNumberSpec_WithoutDoorsNumber_ShouldNotAddVehicle(VehicleType vehicleType)
    {
        // Arrange
        const string manufacturer = "Manufacturer";
        const string model = "Model";
        const int year = 2022;
        const string licensePlate = "12-CD-34";

        var vehiclesRepository = new ServiceRepository<Vehicle>();
        var mockLogger = new Mock<ILogger<VehiclesManagementService>>();

        var vehiclesManagementService = new VehiclesManagementService(vehiclesRepository, mockLogger.Object);

        // Act
        var vehicle = new AddVehicle(manufacturer, model, year, vehicleType, licensePlate);
        var result = vehiclesManagementService.Add(vehicle);

        // Assert
        var addedVehicle = vehiclesRepository.Find(new FindVehicleByLicensePlateSpec(licensePlate));
        addedVehicle.Should().BeNull();
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should()
            .Be($"Number of doors is mandatory for {VehicleType.Hatchback} or {VehicleType.Sedan}.");
    }

    [Fact]
    public void AddVehicleWithSeatsNumberSpec_WithoutSeatsNumber_ShouldNotAddVehicle()
    {
        // Arrange
        const string manufacturer = "Manufacturer";
        const string model = "Model";
        const int year = 2022;
        const string licensePlate = "12-CD-34";
        const VehicleType vehicleType = VehicleType.SUV;

        var vehiclesRepository = new ServiceRepository<Vehicle>();
        var mockLogger = new Mock<ILogger<VehiclesManagementService>>();

        var vehiclesManagementService = new VehiclesManagementService(vehiclesRepository, mockLogger.Object);

        // Act
        var vehicle = new AddVehicle(manufacturer, model, year, vehicleType, licensePlate);
        var result = vehiclesManagementService.Add(vehicle);

        // Assert
        var addedVehicle = vehiclesRepository.Find(new FindVehicleByLicensePlateSpec(licensePlate));
        addedVehicle.Should().BeNull();
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should()
            .Be($"Number of seats is mandatory for {VehicleType.SUV}.");
    }

    [Fact]
    public void AddVehicleWithLoadCapacitySpec_WithoutLoadCapacity_ShouldNotAddVehicle()
    {
        // Arrange
        const string manufacturer = "Manufacturer";
        const string model = "Model";
        const int year = 2022;
        const string licensePlate = "12-CD-34";
        const VehicleType vehicleType = VehicleType.Truck;

        var vehiclesRepository = new ServiceRepository<Vehicle>();
        var mockLogger = new Mock<ILogger<VehiclesManagementService>>();

        var vehiclesManagementService = new VehiclesManagementService(vehiclesRepository, mockLogger.Object);

        // Act
        var vehicle = new AddVehicle(manufacturer, model, year, vehicleType, licensePlate);
        var result = vehiclesManagementService.Add(vehicle);

        // Assert
        var addedVehicle = vehiclesRepository.Find(new FindVehicleByLicensePlateSpec(licensePlate));
        addedVehicle.Should().BeNull();
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should()
            .Be($"Load capacity is mandatory for {VehicleType.Truck}.");
    }

    [Theory]
    [InlineData(VehicleType.Sedan, 5, null)]
    [InlineData(VehicleType.Hatchback, 5, null)]
    [InlineData(VehicleType.SUV, null, 6)]
    public void AddVehicleWithoutLoadCapacitySpec_WithLoadCapacity_ShouldNotAddVehicle(
        VehicleType vehicleType,
        int? doorsNumber,
        int? seatsNumber)
    {
        // Arrange
        const string manufacturer = "Manufacturer";
        const string model = "Model";
        const int year = 2022;
        const string licensePlate = "12-CD-34";

        var vehiclesRepository = new ServiceRepository<Vehicle>();
        var mockLogger = new Mock<ILogger<VehiclesManagementService>>();

        var vehiclesManagementService = new VehiclesManagementService(vehiclesRepository, mockLogger.Object);

        // Act
        var vehicle = new AddVehicle(manufacturer, model, year, vehicleType, licensePlate, doorsNumber, seatsNumber, 10000);
        var result = vehiclesManagementService.Add(vehicle);

        // Assert
        var addedVehicle = vehiclesRepository.Find(new FindVehicleByLicensePlateSpec(licensePlate));
        addedVehicle.Should().BeNull();
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should()
            .Be($"Load capacity is not allowed for {VehicleType.Hatchback}, {VehicleType.Sedan} or {VehicleType.SUV} vehicles.");
    }

    [Theory]
    [InlineData(VehicleType.Hatchback, 5, 6, null)]
    [InlineData(VehicleType.Truck, null, 3, 10000f)]
    public void AddVehicleWithoutSeatsNumberSpec_WithSeatsNumber_ShouldNotAddVehicle(
        VehicleType vehicleType,
        int? doorsNumber,
        int? seatsNumber,
        float? loadCapacity)
    {
        // Arrange
        const string manufacturer = "Manufacturer";
        const string model = "Model";
        const int year = 2022;
        const string licensePlate = "12-CD-34";

        var vehiclesRepository = new ServiceRepository<Vehicle>();
        var mockLogger = new Mock<ILogger<VehiclesManagementService>>();

        var vehiclesManagementService = new VehiclesManagementService(vehiclesRepository, mockLogger.Object);

        // Act
        var vehicle = new AddVehicle(manufacturer, model, year, vehicleType, licensePlate, doorsNumber, seatsNumber, loadCapacity);
        var result = vehiclesManagementService.Add(vehicle);

        // Assert
        var addedVehicle = vehiclesRepository.Find(new FindVehicleByLicensePlateSpec(licensePlate));
        addedVehicle.Should().BeNull();
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should()
            .Be($"Number of seats is not allowed for {VehicleType.Hatchback}, {VehicleType.Sedan} or {VehicleType.Truck} vehicles.");
    }

    [Theory]
    [InlineData(VehicleType.SUV, 5, 6, null)]
    [InlineData(VehicleType.Truck, 5, null, 10000f)]
    public void AddVehicleWithoutDoorsNumberSpec_WithDoorsNumber_ShouldNotAddVehicle(
        VehicleType vehicleType,
        int? doorsNumber,
        int? seatsNumber,
        float? loadCapacity)
    {
        // Arrange
        const string manufacturer = "Manufacturer";
        const string model = "Model";
        const int year = 2022;
        const string licensePlate = "12-CD-34";

        var vehiclesRepository = new ServiceRepository<Vehicle>();
        var mockLogger = new Mock<ILogger<VehiclesManagementService>>();

        var vehiclesManagementService = new VehiclesManagementService(vehiclesRepository, mockLogger.Object);

        // Act
        var vehicle = new AddVehicle(manufacturer, model, year, vehicleType, licensePlate, doorsNumber, seatsNumber, loadCapacity);
        var result = vehiclesManagementService.Add(vehicle);

        // Assert
        var addedVehicle = vehiclesRepository.Find(new FindVehicleByLicensePlateSpec(licensePlate));
        addedVehicle.Should().BeNull();
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should()
            .Be($"Number of doors is not allowed for {VehicleType.SUV}, or {VehicleType.Truck} vehicles.");
    }

    [Theory]
    [InlineData(VehicleType.Hatchback, 10)]
    [InlineData(VehicleType.Hatchback, 0)]
    [InlineData(VehicleType.Sedan, 0)]
    [InlineData(VehicleType.Sedan, 10)]
    public void AddVehicleWithDoorsNumberSpec_WithInvalidDoorsNumber_ShouldNotAddVehicle(VehicleType vehicleType, int doorsNumber)
    {
        // Arrange
        const string manufacturer = "Manufacturer";
        const string model = "Model";
        const int year = 2022;
        const string licensePlate = "12-CD-34";

        var vehiclesRepository = new ServiceRepository<Vehicle>();
        var mockLogger = new Mock<ILogger<VehiclesManagementService>>();

        var vehiclesManagementService = new VehiclesManagementService(vehiclesRepository, mockLogger.Object);

        // Act
        var vehicle = new AddVehicle(manufacturer, model, year, vehicleType, licensePlate, doorsNumber);
        var result = vehiclesManagementService.Add(vehicle);

        // Assert
        var addedVehicle = vehiclesRepository.Find(new FindVehicleByLicensePlateSpec(licensePlate));
        addedVehicle.Should().BeNull();
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should()
            .BeOneOf(
                $"Minimum number of doors for {VehicleType.Hatchback} or {VehicleType.Sedan} is 3.",
                $"Maximum number of doors for {VehicleType.Hatchback} or {VehicleType.Sedan} is 5.");
    }

    [Theory]
    [InlineData(VehicleType.SUV, 10)]
    [InlineData(VehicleType.SUV, 0)]
    public void AddVehicleWithSeatsNumberSpec_WithInvalidSeatsNumber_ShouldNotAddVehicle(VehicleType vehicleType, int seatsNumber)
    {
        // Arrange
        const string manufacturer = "Manufacturer";
        const string model = "Model";
        const int year = 2022;
        const string licensePlate = "12-CD-34";

        var vehiclesRepository = new ServiceRepository<Vehicle>();
        var mockLogger = new Mock<ILogger<VehiclesManagementService>>();

        var vehiclesManagementService = new VehiclesManagementService(vehiclesRepository, mockLogger.Object);

        // Act
        var vehicle = new AddVehicle(manufacturer, model, year, vehicleType, licensePlate, null, seatsNumber);
        var result = vehiclesManagementService.Add(vehicle);

        // Assert
        var addedVehicle = vehiclesRepository.Find(new FindVehicleByLicensePlateSpec(licensePlate));
        addedVehicle.Should().BeNull();
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should()
            .BeOneOf(
                $"Minimum number of seats for a {VehicleType.SUV} is 5.",
                $"Maximum number of seats for a {VehicleType.SUV} is 8.");
    }

    [Theory]
    [InlineData(VehicleType.Truck, 0)]
    [InlineData(VehicleType.Truck, 100000f)]
    public void AddVehicleWithLoadCapacitySpec_WithInvalidLoadCapacity_ShouldNotAddVehicle(VehicleType vehicleType, int loadCapacity)
    {
        // Arrange
        const string manufacturer = "Manufacturer";
        const string model = "Model";
        const int year = 2022;
        const string licensePlate = "12-CD-34";

        var vehiclesRepository = new ServiceRepository<Vehicle>();
        var mockLogger = new Mock<ILogger<VehiclesManagementService>>();

        var vehiclesManagementService = new VehiclesManagementService(vehiclesRepository, mockLogger.Object);

        // Act
        var vehicle = new AddVehicle(manufacturer, model, year, vehicleType, licensePlate, null, null, loadCapacity);
        var result = vehiclesManagementService.Add(vehicle);

        // Assert
        var addedVehicle = vehiclesRepository.Find(new FindVehicleByLicensePlateSpec(licensePlate));
        addedVehicle.Should().BeNull();
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should()
            .BeOneOf(
                $"Minimum load capacity for a {VehicleType.Truck} is 10000.",
                $"Maximum load capacity for a {VehicleType.Truck} is 50000.");
    }

    [Theory]
    [InlineData(VehicleType.Sedan, 3067, 5, null, null)]
    [InlineData(VehicleType.Hatchback, 3025, 5, null, null)]
    [InlineData(VehicleType.SUV, 4506, null, 6, null)]
    [InlineData(VehicleType.Truck, 2067, null, null, 10000f)]
    public void AddVehicle_WithInvalidYear_ShouldNotAddVehicle(
        VehicleType vehicleType,
        int year,
        int? doorsNumber,
        int? seatsNumber,
        float? loadCapacity)
    {
        // Arrange
        const string manufacturer = "Manufacturer";
        const string model = "Model";
        const string licensePlate = "12-CD-34";

        var vehiclesRepository = new ServiceRepository<Vehicle>();
        var mockLogger = new Mock<ILogger<VehiclesManagementService>>();

        var vehiclesManagementService = new VehiclesManagementService(vehiclesRepository, mockLogger.Object);

        // Act
        var vehicle = new AddVehicle(
            manufacturer,
            model,
            year,
            vehicleType,
            licensePlate,
            doorsNumber,
            seatsNumber,
            loadCapacity);

        var result = vehiclesManagementService.Add(vehicle);

        // Assert
        // Assert
        var addedVehicle = vehiclesRepository.Find(new FindVehicleByLicensePlateSpec(licensePlate));
        addedVehicle.Should().BeNull();
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be("Invalid vehicle year");
    }

    [Fact]
    public void AddVehicle_FoundVehicleWithSameLicensePlate_ShouldReturnError()
    {
        // Arrange
        const int doorsNumber = 4;
        const string manufacturer = "Manufacturer";
        const string model = "Model";
        const int year = 2022;
        const VehicleType type = VehicleType.Sedan;
        const string licensePlate = "12-CD-34";

        var vehiclesRepository = new ServiceRepository<Vehicle>();
        var mockLogger = new Mock<ILogger<VehiclesManagementService>>();

        var vehiclesManagementService = new VehiclesManagementService(vehiclesRepository, mockLogger.Object);

        // Act
        var vehicle = new AddVehicle(manufacturer, model, year, type, licensePlate, doorsNumber);
        vehiclesManagementService.Add(vehicle);
        var result = vehiclesManagementService.Add(vehicle);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Message.Should().Be($"Vehicle with License Plate {licensePlate} already exists.");
    }

    [Theory]
    [InlineData(VehicleType.Sedan, 5, null, null)]
    [InlineData(VehicleType.Hatchback, 5, null, null)]
    [InlineData(VehicleType.SUV, null, 6, null)]
    [InlineData(VehicleType.Truck, null, null, 10000f)]
    public void SearchVehicle_ByVehicleType_ShouldReturnResults(
        VehicleType vehicleType,
        int? doorsNumber,
        int? seatsNumber,
        float? loadCapacity)
    {
        // Arrange
        const string manufacturer = "Manufacturer";
        const string model = "Model";
        const int year = 2022;
        const string licensePlate = "12-CD-34";

        var vehiclesRepository = new ServiceRepository<Vehicle>();
        var mockLogger = new Mock<ILogger<VehiclesManagementService>>();

        var vehiclesManagementService = new VehiclesManagementService(vehiclesRepository, mockLogger.Object);

        // Act
        var vehicle = new AddVehicle(
            manufacturer,
            model,
            year,
            vehicleType,
            licensePlate,
            doorsNumber,
            seatsNumber,
            loadCapacity);

        vehiclesManagementService.Add(vehicle);
        var result = vehiclesManagementService.Search(
            vehicleType,
            null,
            null,
            null).ToList();

        // Assert
        result.Count.Should().Be(1);
        result[0].Type.Should().Be(vehicleType);
    }

    [Theory]
    [InlineData(VehicleType.Sedan, 5, null, null)]
    [InlineData(VehicleType.Hatchback, 5, null, null)]
    [InlineData(VehicleType.SUV, null, 6, null)]
    [InlineData(VehicleType.Truck, null, null, 10000f)]
    public void SearchVehicle_ByManufacturer_ShouldReturnResults(
        VehicleType vehicleType,
        int? doorsNumber,
        int? seatsNumber,
        float? loadCapacity)
    {
        // Arrange
        const string manufacturer = "Manufacturer";
        const string model = "Model";
        const int year = 2022;
        const string licensePlate = "12-CD-34";

        var vehiclesRepository = new ServiceRepository<Vehicle>();
        var mockLogger = new Mock<ILogger<VehiclesManagementService>>();

        var vehiclesManagementService = new VehiclesManagementService(vehiclesRepository, mockLogger.Object);

        // Act
        var vehicle = new AddVehicle(
            manufacturer,
            model,
            year,
            vehicleType,
            licensePlate,
            doorsNumber,
            seatsNumber,
            loadCapacity);

        vehiclesManagementService.Add(vehicle);
        var result = vehiclesManagementService.Search(
            null,
            manufacturer,
            null,
            null).ToList();

        // Assert
        result.Count.Should().Be(1);
        result[0].Manufacturer.Should().Be(manufacturer);
    }

    [Theory]
    [InlineData(VehicleType.Sedan, 5, null, null)]
    [InlineData(VehicleType.Hatchback, 5, null, null)]
    [InlineData(VehicleType.SUV, null, 6, null)]
    [InlineData(VehicleType.Truck, null, null, 10000f)]
    public void SearchVehicle_ByModel_ShouldReturnResults(
        VehicleType vehicleType,
        int? doorsNumber,
        int? seatsNumber,
        float? loadCapacity)
    {
        // Arrange
        const string manufacturer = "Manufacturer";
        const string model = "Model";
        const int year = 2022;
        const string licensePlate = "12-CD-34";

        var vehiclesRepository = new ServiceRepository<Vehicle>();
        var mockLogger = new Mock<ILogger<VehiclesManagementService>>();

        var vehiclesManagementService = new VehiclesManagementService(vehiclesRepository, mockLogger.Object);

        // Act
        var vehicle = new AddVehicle(
            manufacturer,
            model,
            year,
            vehicleType,
            licensePlate,
            doorsNumber,
            seatsNumber,
            loadCapacity);

        vehiclesManagementService.Add(vehicle);
        var result = vehiclesManagementService.Search(
            null,
            null,
            model,
            null).ToList();

        // Assert
        result.Count.Should().Be(1);
        result[0].Model.Should().Be(model);
    }

    [Theory]
    [InlineData(VehicleType.Sedan, 5, null, null)]
    [InlineData(VehicleType.Hatchback, 5, null, null)]
    [InlineData(VehicleType.SUV, null, 6, null)]
    [InlineData(VehicleType.Truck, null, null, 10000f)]
    public void SearchVehicle_ByYear_ShouldReturnResults(
        VehicleType vehicleType,
        int? doorsNumber,
        int? seatsNumber,
        float? loadCapacity)
    {
        // Arrange
        const string manufacturer = "Manufacturer";
        const string model = "Model";
        const int year = 2022;
        const string licensePlate = "12-CD-34";

        var vehiclesRepository = new ServiceRepository<Vehicle>();
        var mockLogger = new Mock<ILogger<VehiclesManagementService>>();

        var vehiclesManagementService = new VehiclesManagementService(vehiclesRepository, mockLogger.Object);

        // Act
        var vehicle = new AddVehicle(
            manufacturer,
            model,
            year,
            vehicleType,
            licensePlate,
            doorsNumber,
            seatsNumber,
            loadCapacity);

        vehiclesManagementService.Add(vehicle);
        var result = vehiclesManagementService.Search(
            null,
            null,
            null,
            year).ToList();

        // Assert
        result.Count.Should().Be(1);
        result[0].Year.Should().Be(year);
    }

    [Theory]
    [InlineData(VehicleType.Sedan, 5, null, null)]
    [InlineData(VehicleType.Hatchback, 5, null, null)]
    [InlineData(VehicleType.SUV, null, 6, null)]
    [InlineData(VehicleType.Truck, null, null, 10000f)]
    public void SearchVehicle_ByAllSearchableFields_ShouldReturnResults(
        VehicleType vehicleType,
        int? doorsNumber,
        int? seatsNumber,
        float? loadCapacity)
    {
        // Arrange
        const string manufacturer = "Manufacturer";
        const string model = "Model";
        const int year = 2022;
        const string licensePlate = "12-CD-34";

        var vehiclesRepository = new ServiceRepository<Vehicle>();
        var mockLogger = new Mock<ILogger<VehiclesManagementService>>();

        var vehiclesManagementService = new VehiclesManagementService(vehiclesRepository, mockLogger.Object);

        // Act
        var vehicle = new AddVehicle(
            manufacturer,
            model,
            year,
            vehicleType,
            licensePlate,
            doorsNumber,
            seatsNumber,
            loadCapacity);

        vehiclesManagementService.Add(vehicle);
        var result = vehiclesManagementService.Search(
            vehicleType,
            manufacturer,
            model,
            year).ToList();

        // Assert
        result.Count.Should().Be(1);
        result[0].Type.Should().Be(vehicleType);
        result[0].Manufacturer.Should().Be(manufacturer);
        result[0].Model.Should().Be(model);
        result[0].Year.Should().Be(year);
    }
}