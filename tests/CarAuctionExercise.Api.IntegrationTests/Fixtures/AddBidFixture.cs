namespace CarAuctionExercise.Api.IntegrationTests.Fixtures;

using CarAuctionExercise.Application.DTOs.Bids;

public static class AddBidFixture
{
    public static AddBid GetAddBid(string auctionId)
    {
        return new AddBid(2000, auctionId, "bidderName");
    }
}