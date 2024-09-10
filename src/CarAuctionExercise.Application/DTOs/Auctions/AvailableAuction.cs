namespace CarAuctionExercise.Application.DTOs.Auctions;

using CarAuctionExercise.Application.DTOs.Vehicles;
using CarAuctionExercise.Domain;

public record AvailableAuction(
    string Id,
    AvailableVehicle Vehicle,
    DateTime? StartDate,
    DateTime? CloseDate,
    float StartingBid,
    IEnumerable<Bid> Bids,
    bool Active);