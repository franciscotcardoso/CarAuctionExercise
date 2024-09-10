namespace CarAuctionExercise.Application.DTOs.Vehicles;

using CarAuctionExercise.Domain;

public record AddVehicle(
    string Manufacturer,
    string Model,
    int Year,
    VehicleType VehicleType,
    string LicensePlate,
    int? DoorsNumber = null,
    int? SeatsNumber = null,
    float? LoadCapacity = null
);
