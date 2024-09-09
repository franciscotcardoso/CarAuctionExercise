using CarAuctionExercise.Domain;
using CarAuctionExercise.Infrastructure.Abstractions;

namespace CarAuctionExercise.Application.Specifications.Vehicles;

public class FindVehicleByLicensePlateSpec : Specification<Vehicle>
{
    public FindVehicleByLicensePlateSpec(string licensePlate) : base(
        x => x.LicensePlate == licensePlate)
    {
    }
}