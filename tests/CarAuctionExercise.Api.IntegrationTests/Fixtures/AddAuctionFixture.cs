namespace CarAuctionExercise.Api.IntegrationTests.Fixtures;

using CarAuctionExercise.Application.DTOs.Auctions;

public static class AddAuctionFixture
{
    public static AddAuction GetAddAuction(
        float startingBid = 1000,
        string licensePlate = "licensePlate1")
    {
        return new AddAuction(startingBid, licensePlate);
    }
}