namespace CarAuctionExercise.Application.DTOs.Vehicles;

using CarAuctionExercise.Domain;

public record AvailableVehicle(
    string Manufacturer,
    string Model, 
    int Year,
    VehicleType Type,
    string LicensePlate);