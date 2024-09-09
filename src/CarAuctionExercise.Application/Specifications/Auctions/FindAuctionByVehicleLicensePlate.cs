using CarAuctionExercise.Domain;
using CarAuctionExercise.Infrastructure.Abstractions;

namespace CarAuctionExercise.Application.Specifications.Auctions;

public class FindAuctionByVehicleLicensePlate : Specification<Auction>
{
    public FindAuctionByVehicleLicensePlate(string licensePlate) : base(
        x => x.Vehicle.LicensePlate == licensePlate)
    {
    }
}