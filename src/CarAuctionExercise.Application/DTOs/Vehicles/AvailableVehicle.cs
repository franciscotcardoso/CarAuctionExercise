using CarAuctionExercise.Domain;

namespace CarAuctionExercise.Application.DTOs.Vehicles;

public record AvailableVehicle(
    string Manufacturer,
    string Model, 
    int Year,
    VehicleType Type,
    string LicensePlate);