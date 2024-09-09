using CarAuctionExercise.Application.DTOs.Vehicles;
using CarAuctionExercise.Application.Interfaces;
using CarAuctionExercise.Application.Mappers;
using CarAuctionExercise.Application.Specifications.Vehicles;
using CarAuctionExercise.Application.Validators;
using CarAuctionExercise.Domain;
using CarAuctionExercise.Infrastructure.Interfaces;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace CarAuctionExercise.Application.Services;

public class VehiclesManagementService : IVehiclesManagementService
{
    private readonly IServiceRepository<Vehicle> _vehiclesRepository;
    private readonly ILogger<VehiclesManagementService> _logger;

    public VehiclesManagementService(
        IServiceRepository<Vehicle> vehiclesRepository,
        ILogger<VehiclesManagementService> logger)
    {
        _vehiclesRepository = vehiclesRepository;
        _logger = logger;
    }

    public Result<AvailableVehicle> Add(AddVehicle vehicle)
    {
        if (SearchByLicensePlate(vehicle.LicensePlate))
        {
            return Result.Fail($"Vehicle with License Plate {vehicle.LicensePlate} already exists.");
        }
        
        var vehicleValidator = new VehicleValidator();
        var result = vehicleValidator.Validate(vehicle);

        if (result.IsValid)
        {
            var addedVehicle = _vehiclesRepository.Add(vehicle.MapToVehicle());
            
            _logger.LogInformation(
                "Added vehicle with license plate {LicensePlate} to inventory",
                addedVehicle.LicensePlate);

            return addedVehicle.MapToAvailableVehicle();
        }
        
        var errors = result.Errors
            .Select(x => x.ErrorMessage)
            .ToList();

        return Result.Fail<AvailableVehicle>(errors);
    }
    
    public IEnumerable<AvailableVehicle> Search(VehicleType? vehicleType, string? manufacturer, string? model, int? year)
    {
        var spec = new FindVehiclesByMultipleParameters(vehicleType, manufacturer, model, year);
        var vehicles = _vehiclesRepository.FindAll(spec).ToList();

        return vehicles.Select(x => x.MapToAvailableVehicle());
    }

    private bool SearchByLicensePlate(string licensePlate)
    {
        var spec = new FindVehicleByLicensePlateSpec(licensePlate);
        return _vehiclesRepository.Find(spec) is not null;
    }
    
}