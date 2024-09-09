using CarAuctionExercise.Domain;
using CarAuctionExercise.Infrastructure.Abstractions;

namespace CarAuctionExercise.Application.Specifications.Auctions;

public class FindAuctionByIdSpec : Specification<Auction>
{
    public FindAuctionByIdSpec(string auctionId) : base(
        x => x.Id == auctionId)
    {
    }
}