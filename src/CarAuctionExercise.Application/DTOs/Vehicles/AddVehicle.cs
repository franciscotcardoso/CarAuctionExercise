using CarAuctionExercise.Domain;

namespace CarAuctionExercise.Application.DTOs.Vehicles;

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
