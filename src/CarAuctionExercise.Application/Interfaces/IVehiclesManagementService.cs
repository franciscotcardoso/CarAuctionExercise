using CarAuctionExercise.Application.DTOs.Vehicles;
using CarAuctionExercise.Domain;
using FluentResults;

namespace CarAuctionExercise.Application.Interfaces;

public interface IVehiclesManagementService
{
    public Result<AvailableVehicle> Add(AddVehicle vehicle);

    public IEnumerable<AvailableVehicle> Search(
        VehicleType? vehicleType, 
        string? manufacturer,
        string? model, 
        int? year);
}