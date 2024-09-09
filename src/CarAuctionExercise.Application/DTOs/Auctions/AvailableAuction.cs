using CarAuctionExercise.Application.DTOs.Vehicles;
using CarAuctionExercise.Domain;

namespace CarAuctionExercise.Application.DTOs.Auctions;

public record AvailableAuction(
    String Id,
    AvailableVehicle Vehicle,
    DateTime? StartDate,
    DateTime? CloseDate,
    float StartingBid,
    IEnumerable<Bid> Bids,
    bool Active);