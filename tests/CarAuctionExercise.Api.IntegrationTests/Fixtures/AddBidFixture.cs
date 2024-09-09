using CarAuctionExercise.Application.DTOs.Bids;

namespace CarAuctionExercise.Api.IntegrationTests.Fixtures;

public static class AddBidFixture
{
    public static AddBid GetAddBid(string auctionId)
    {
        return new AddBid(2000, auctionId, "bidderName");
    }
}