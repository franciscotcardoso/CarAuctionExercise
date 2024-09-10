namespace CarAuctionExercise.Api.IntegrationTests.Fixtures;

using CarAuctionExercise.Application.DTOs.Vehicles;
using CarAuctionExercise.Domain;

public static class AddVehicleFixture
{
    public static AddVehicle GetAddVehicle(
        string manufacturer = "manufacturer",
        string model = "model",
        int year = 2022,
        VehicleType vehicleType = VehicleType.Sedan,
        string licensePlate = "licensePlate",
        int? doorsNumber = 5,
        int? seatsNumber = null,
        float? loadCapacity = null)
    {
        return new AddVehicle(
            manufacturer,
            model,
            year,
            vehicleType,
            licensePlate,
            doorsNumber,
            seatsNumber,
            loadCapacity);
    }
}