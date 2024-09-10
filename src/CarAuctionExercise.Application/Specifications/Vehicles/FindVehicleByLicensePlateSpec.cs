namespace CarAuctionExercise.Application.Specifications.Vehicles;

using CarAuctionExercise.Domain;
using CarAuctionExercise.Infrastructure.Abstractions;

public class FindVehicleByLicensePlateSpec : Specification<Vehicle>
{
    public FindVehicleByLicensePlateSpec(string licensePlate)
        : base(
        x => x.LicensePlate == licensePlate)
    {
    }
}