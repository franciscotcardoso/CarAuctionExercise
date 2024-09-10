namespace CarAuctionExercise.Application.Specifications.Auctions;

using CarAuctionExercise.Domain;
using CarAuctionExercise.Infrastructure.Abstractions;

public class FindAuctionByIdSpec : Specification<Auction>
{
    public FindAuctionByIdSpec(string auctionId)
        : base(x => x.Id == auctionId)
    {
    }
}