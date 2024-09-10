namespace CarAuctionExercise.Application.Mappers;

using CarAuctionExercise.Application.DTOs.Auctions;
using CarAuctionExercise.Application.DTOs.Vehicles;
using CarAuctionExercise.Domain;

public static class AuctionMappers
{
    public static AvailableAuction MapToAvailableAuction(this Auction auction)
    {
        var availableVehicle = new AvailableVehicle(
            auction.Vehicle.Manufacturer,
            auction.Vehicle.Model,
            auction.Vehicle.Year,
            auction.Vehicle.Type,
            auction.Vehicle.LicensePlate);

        return new AvailableAuction(
            auction.Id,
            availableVehicle,
            auction.StartDate,
            auction.CloseDate,
            auction.StartingBid,
            auction.Bids,
            auction.Active);
    }
}