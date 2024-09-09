using CarAuctionExercise.Application.DTOs.Auctions;

namespace CarAuctionExercise.Api.IntegrationTests.Fixtures;

public static class AddAuctionFixture
{
    public static AddAuction GetAddAuction(
        float startingBid = 1000,
        string licensePlate = "licensePlate1")
    {
        return new AddAuction(startingBid, licensePlate);
    }
}