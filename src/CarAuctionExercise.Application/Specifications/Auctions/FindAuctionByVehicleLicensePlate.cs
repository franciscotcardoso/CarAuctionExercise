namespace CarAuctionExercise.Application.Specifications.Auctions;

using CarAuctionExercise.Domain;
using CarAuctionExercise.Infrastructure.Abstractions;

public class FindAuctionByVehicleLicensePlate : Specification<Auction>
{
    public FindAuctionByVehicleLicensePlate(string licensePlate)
        : base(
        x => x.Vehicle.LicensePlate == licensePlate)
    {
    }
}