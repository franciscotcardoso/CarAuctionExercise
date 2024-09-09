using CarAuctionExercise.Application.DTOs.Auctions;
using CarAuctionExercise.Application.DTOs.Bids;
using CarAuctionExercise.Application.DTOs.Vehicles;
using CarAuctionExercise.Application.Services;
using CarAuctionExercise.Application.Specifications.Auctions;
using CarAuctionExercise.Domain;
using CarAuctionExercise.Infrastructure.Data;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CarAuctionExercise.Application.UnitTests.Services;

public class AuctionsManagementServiceTests
{
    [Fact]
    public void AddAuction_WithValidData_ShouldAddAuction()
    {
        // Arrange
        var addAuction = SetupAuctionServicesAndRepositories(
            out var addVehicle,
            out var auctionsManagementService,
            out var vehiclesManagementService,
            out var auctionsRepository);
        
        // Act
        vehiclesManagementService.Add(addVehicle);
        var result = auctionsManagementService.Add(addAuction);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var addedAuction = auctionsRepository.Find(new FindAuctionByIdSpec(result.Value.Id));
        addedAuction.Should().NotBeNull();
        addedAuction!.Id.Should().Be(result.Value.Id);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void AddAuction_WithInvalidStartingBidData_ShouldReturnError(float startingBid)
    {
        // Arrange
        var addAuction = SetupAuctionServicesAndRepositories(
            out var addVehicle,
            out var auctionsManagementService,
            out var vehiclesManagementService,
            out _,
            startingBid);
        
        // Act
        vehiclesManagementService.Add(addVehicle);
        var result = auctionsManagementService.Add(addAuction);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be("Invalid starting bid value.");
    }
    
    [Fact]
    public void AddAuction_WhenAuctionAlreadyExists_ShouldReturnError()
    {
        // Arrange
        var addAuction = SetupAuctionServicesAndRepositories(
            out var addVehicle,
            out var auctionsManagementService,
            out var vehiclesManagementService,
            out _);
        
        // Act
        vehiclesManagementService.Add(addVehicle);
        auctionsManagementService.Add(addAuction);
        var result = auctionsManagementService.Add(addAuction);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should()
            .Be($"Auction for vehicle with license plate {addVehicle.LicensePlate} already exists.");
    }
    
    [Fact]
    public void AddAuction_ForNonExistentVehicle_ShouldReturnError()
    {
        // Arrange
        var addAuction = SetupAuctionServicesAndRepositories(
            out var addVehicle,
            out var auctionsManagementService,
            out var vehiclesManagementService,
            out _);

        addAuction = addAuction with { LicensePlate = "OverridingLicensePlace" };
        
        // Act
        vehiclesManagementService.Add(addVehicle);
        auctionsManagementService.Add(addAuction);
        var result = auctionsManagementService.Add(addAuction);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should()
            .Be($"Vehicle with license plate {addAuction.LicensePlate} not found.");
    }
    
    [Fact]
    public void StartAuction_ForValidAuction_ShouldStartAuction()
    {
        // Arrange
        var addAuction = SetupAuctionServicesAndRepositories(
            out var addVehicle,
            out var auctionsManagementService,
            out var vehiclesManagementService,
            out var auctionsRepository);
        
        // Act
        vehiclesManagementService.Add(addVehicle);
        var result = auctionsManagementService.Add(addAuction);
        auctionsManagementService.Start(result.Value.Id);

        // Assert
        var addedAuction = auctionsRepository.Find(new FindAuctionByIdSpec(result.Value.Id));
        addedAuction.Should().NotBeNull();
        addedAuction!.Active.Should().BeTrue();
    }
    
    [Fact]
    public void StartAuction_ForInValidAuctionId_ShouldNotStartAuction()
    {
        // Arrange
        const string invalidAuctionId = "InvalidAuctionId";
        
        var vehiclesRepository = new ServiceRepository<Vehicle>();
        var auctionsRepository = new ServiceRepository<Auction>();
        var mockAuctionsManagementServiceLogger = new Mock<ILogger<AuctionsManagementService>>();
        var auctionsManagementService = new AuctionsManagementService(
            auctionsRepository,
            vehiclesRepository, 
            mockAuctionsManagementServiceLogger.Object);

        // Act

        var result = auctionsManagementService.Start(invalidAuctionId);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be("Auction not found.");
    }
    
    [Fact]
    public void CloseAuction_ForStartedAuction_ShouldClosedAuction()
    {
        // Arrange
        var addAuction = SetupAuctionServicesAndRepositories(
            out var addVehicle,
            out var auctionsManagementService,
            out var vehiclesManagementService,
            out var auctionsRepository);
        
        // Act
        vehiclesManagementService.Add(addVehicle);
        var result = auctionsManagementService.Add(addAuction);
        auctionsManagementService.Start(result.Value.Id);
        auctionsManagementService.Close(result.Value.Id);

        // Assert
        var addedAuction = auctionsRepository.Find(new FindAuctionByIdSpec(result.Value.Id));
        addedAuction.Should().NotBeNull();
        addedAuction!.Active.Should().BeFalse();
    }
    
    [Fact]
    public void CloseAuction_ForInValidAuctionId_ShouldNotCloseAuction()
    {
        // Arrange
        const string invalidAuctionId = "InvalidAuctionId";
        
        var vehiclesRepository = new ServiceRepository<Vehicle>();
        var auctionsRepository = new ServiceRepository<Auction>();
        var mockAuctionsManagementServiceLogger = new Mock<ILogger<AuctionsManagementService>>();
        var auctionsManagementService = new AuctionsManagementService(
            auctionsRepository,
            vehiclesRepository, 
            mockAuctionsManagementServiceLogger.Object);

        // Act
        auctionsManagementService.Start(invalidAuctionId);
        var result = auctionsManagementService.Close(invalidAuctionId);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be("Auction not found.");
    }
    
    [Fact]
    public void StartAuction_ForAlreadyClosedAuction_ShouldNotStartAuction()
    {
        // Arrange
        var addAuction = SetupAuctionServicesAndRepositories(
            out var addVehicle,
            out var auctionsManagementService,
            out var vehiclesManagementService,
            out _);
        
        // Act
        vehiclesManagementService.Add(addVehicle);
        var addedAuction = auctionsManagementService.Add(addAuction);
        auctionsManagementService.Start(addedAuction.Value.Id);
        auctionsManagementService.Close(addedAuction.Value.Id);
        var result = auctionsManagementService.Start(addedAuction.Value.Id);

        // Assert
        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be("Auction already closed.");
    }
    
    [Fact]
    public void CloseAuction_ForNotStartedAuction_ShouldNotCloseAuction()
    {
        // Arrange
        var addAuction = SetupAuctionServicesAndRepositories(
            out var addVehicle,
            out var auctionsManagementService,
            out var vehiclesManagementService,
            out _);

        // Act
        vehiclesManagementService.Add(addVehicle);
        var addedAuction = auctionsManagementService.Add(addAuction);
        var result = auctionsManagementService.Close(addedAuction.Value.Id);
        
        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be("Auction not started yet.");
    }
    
    [Fact]
    public void PlaceBid_WithValidData_ShouldAddBid()
    {
        // Arrange
        const string bidder = "BidderName";
        
        var addAuction = SetupAuctionServicesAndRepositories(
            out var addVehicle,
            out var auctionsManagementService,
            out var vehiclesManagementService,
            out var auctionsRepository);
        
        vehiclesManagementService.Add(addVehicle);
        var addedAuctionResult = auctionsManagementService.Add(addAuction);
        var addBid = new AddBid(3000, addedAuctionResult.Value.Id, bidder);
        var auctionId = addedAuctionResult.Value.Id;
        auctionsManagementService.Start(auctionId);
        
        // Act
        var result = auctionsManagementService.Bid(addBid);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var auctionWithBid = auctionsRepository.Find(new FindAuctionByIdSpec(auctionId));
        auctionWithBid.Should().NotBeNull();
        auctionWithBid!.Bids.Count.Should().Be(1);
    }
    
    [Fact]
    public void PlaceBid_ToInvalidAuction_ShouldReturnError()
    {
        // Arrange
        const string bidder = "BidderName";
        const string invalidAuctionId = "invalidAuctionId";
        
        SetupAuctionServicesAndRepositories(
            out _,
            out var auctionsManagementService,
            out _,
            out _);
        
        var addBid = new AddBid(3000, invalidAuctionId, bidder);
        
        // Act
        var result = auctionsManagementService.Bid(addBid);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be("Auction not found.");
    }
    
    [Theory]
    [InlineData(5)]  // Less that starting bid
    [InlineData(10)] // Starting bid
    [InlineData(1000)] // Less than first bid but great that starting bid
    [InlineData(3000)] // First bid
    public void PlaceBid_WithInvalidValue_ShouldReturnError(float bid)
    {
        // Arrange
        const string bidder = "BidderName";
        const float startingBid = 10;
        
        var addAuction = SetupAuctionServicesAndRepositories(
            out var addVehicle,
            out var auctionsManagementService,
            out var vehiclesManagementService,
            out _,
            startingBid);
        
        vehiclesManagementService.Add(addVehicle);
        var addedAuctionResult = auctionsManagementService.Add(addAuction);
        var startBid = new AddBid(3000, addedAuctionResult.Value.Id, bidder);
        var nextBid = new AddBid(bid, addedAuctionResult.Value.Id, bidder);
        var auctionId = addedAuctionResult.Value.Id;
        auctionsManagementService.Start(auctionId);
        
        // Act
        auctionsManagementService.Bid(startBid);
        var result = auctionsManagementService.Bid(nextBid);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().BeOneOf(
            "Bid value is less or equal than the starting bid value.",
            "Bid value is less or equal than the previous bid.");
    }
    
    [Fact]
    public void GetAuctions_WithCreatedAuctions_ShouldReturnAuctions()
    {
        // Arrange
        var addAuction = SetupAuctionServicesAndRepositories(
            out var addVehicle,
            out var auctionsManagementService,
            out var vehiclesManagementService,
            out _);
        
        // Act
        vehiclesManagementService.Add(addVehicle);
        auctionsManagementService.Add(addAuction);
        var auctions = auctionsManagementService.GetAllAuctions();

        // Assert
        auctions.Count().Should().Be(1);
    }
    
    [Fact]
    public void GetAuctions_WithoutCreatedAuctions_ShouldReturnEmptyList()
    {
        // Arrange
        SetupAuctionServicesAndRepositories(
            out _,
            out var auctionsManagementService,
            out _,
            out _);
        
        // Act
        var auctions = auctionsManagementService.GetAllAuctions();

        // Assert
        auctions.Count().Should().Be(0);
    }

    private static AddAuction SetupAuctionServicesAndRepositories(
        out AddVehicle addVehicle,
        out AuctionsManagementService auctionsManagementService,
        out VehiclesManagementService vehiclesManagementService, 
        out ServiceRepository<Auction> auctionsRepository,
        float startingBid = 100
        )
    {
        const string manufacturer = "Manufacturer";
        const string model = "Model";
        const int year = 2022;
        const string licensePlate = "12-CD-34";
        var vehicleType = VehicleType.Hatchback;
        const int doorsNumber = 5;

        var addAuction = new AddAuction(startingBid, licensePlate);
        addVehicle = new AddVehicle(
            manufacturer,
            model,
            year,
            vehicleType,
            licensePlate,
            doorsNumber);
        
        var vehiclesRepository = new ServiceRepository<Vehicle>();
        auctionsRepository = new ServiceRepository<Auction>();
        var mockAuctionsManagementServiceLogger = new Mock<ILogger<AuctionsManagementService>>();
        var mockVehiclesManagementServiceLogger = new Mock<ILogger<VehiclesManagementService>>();

        auctionsManagementService = new AuctionsManagementService(
            auctionsRepository,
            vehiclesRepository,
            mockAuctionsManagementServiceLogger.Object);
        
        vehiclesManagementService = new VehiclesManagementService(
            vehiclesRepository,
            mockVehiclesManagementServiceLogger.Object);
        return addAuction;
    }
}