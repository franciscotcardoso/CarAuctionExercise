namespace CarAuctionExercise.Application.Validators;

using CarAuctionExercise.Application.DTOs.Auctions;
using FluentValidation;

public class AuctionValidator : AbstractValidator<AddAuction>
{
    public AuctionValidator()
    {
        RuleFor(x => x.StartingBid)
            .GreaterThan(0)
            .WithMessage("Invalid starting bid value.");
    }
}