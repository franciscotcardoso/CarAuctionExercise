namespace CarAuctionExercise.Application.Interfaces;

using CarAuctionExercise.Application.DTOs.Auctions;
using CarAuctionExercise.Application.DTOs.Bids;
using FluentResults;

public interface IAuctionsManagementService
{
    public Result<AvailableAuction> Add(AddAuction auction);

    public IEnumerable<AvailableAuction> GetAllAuctions();

    public Result Start(string auctionId);

    public Result Close(string auctionId);

    public Result Bid(AddBid bid);
}