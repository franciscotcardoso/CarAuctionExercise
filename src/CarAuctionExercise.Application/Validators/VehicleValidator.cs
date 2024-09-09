using CarAuctionExercise.Application.DTOs.Vehicles;
using CarAuctionExercise.Domain;
using FluentValidation;

namespace CarAuctionExercise.Application.Validators;

public class VehicleValidator : AbstractValidator<AddVehicle>
{
    public VehicleValidator()
    {
        RuleFor(x => x.Year)
            .Cascade(CascadeMode.Stop)
            .LessThanOrEqualTo(DateTime.Now.Year)
            .WithMessage("Invalid vehicle year");
        
        RuleFor(x => x.DoorsNumber)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .When(x => x.VehicleType is VehicleType.Hatchback or VehicleType.Sedan)
            .WithMessage($"Number of doors is mandatory for {VehicleType.Hatchback} or {VehicleType.Sedan}.");
        
        RuleFor(x => x.DoorsNumber)
            .Cascade(CascadeMode.Stop)
            .GreaterThanOrEqualTo(3)
            .When(x => x.VehicleType is VehicleType.Hatchback or VehicleType.Sedan)
            .WithMessage($"Minimum number of doors for {VehicleType.Hatchback} or {VehicleType.Sedan} is 3.");
        
        RuleFor(x => x.DoorsNumber)
            .Cascade(CascadeMode.Stop)
            .LessThanOrEqualTo(5)
            .When(x => x.VehicleType is VehicleType.Hatchback or VehicleType.Sedan)
            .WithMessage($"Maximum number of doors for {VehicleType.Hatchback} or {VehicleType.Sedan} is 5.");

        RuleFor(x => x.SeatsNumber)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .When(x => x.VehicleType is VehicleType.SUV)
            .WithMessage($"Number of seats is mandatory for {VehicleType.SUV}.");
        
        RuleFor(x => x.SeatsNumber)
            .Cascade(CascadeMode.Stop)
            .GreaterThanOrEqualTo(5)
            .When(x => x.VehicleType is VehicleType.SUV)
            .WithMessage($"Minimum number of seats for a {VehicleType.SUV} is 5.");
        
        RuleFor(x => x.SeatsNumber)
            .Cascade(CascadeMode.Stop)
            .LessThanOrEqualTo(8)
            .When(x => x.VehicleType is VehicleType.SUV)
            .WithMessage($"Maximum number of seats for a {VehicleType.SUV} is 8.");

        RuleFor(x => x.LoadCapacity)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .When(x => x.VehicleType is VehicleType.Truck)
            .WithMessage($"Load capacity is mandatory for {VehicleType.Truck}.");
        
        RuleFor(x => x.LoadCapacity)
            .Cascade(CascadeMode.Stop)
            .GreaterThanOrEqualTo(10000)
            .When(x => x.VehicleType is VehicleType.Truck)
            .WithMessage($"Minimum load capacity for a {VehicleType.Truck} is 10000.");
        
        RuleFor(x => x.LoadCapacity)
            .Cascade(CascadeMode.Stop)
            .LessThanOrEqualTo(50000)
            .When(x => x.VehicleType is VehicleType.Truck)
            .WithMessage($"Maximum load capacity for a {VehicleType.Truck} is 50000.");
        
        RuleFor(x => x.LoadCapacity)
            .Cascade(CascadeMode.Stop)
            .Null()
            .When(x => x.VehicleType is VehicleType.Hatchback or VehicleType.Sedan or VehicleType.SUV)
            .WithMessage($"Load capacity is not allowed for {VehicleType.Hatchback}, {VehicleType.Sedan} or {VehicleType.SUV} vehicles.");
        
        RuleFor(x => x.SeatsNumber)
            .Cascade(CascadeMode.Stop)
            .Null()
            .When(x => x.VehicleType is VehicleType.Hatchback or VehicleType.Sedan or VehicleType.Truck)
            .WithMessage($"Number of seats is not allowed for {VehicleType.Hatchback}, {VehicleType.Sedan} or {VehicleType.Truck} vehicles.");
        
        RuleFor(x => x.DoorsNumber)
            .Cascade(CascadeMode.Stop)
            .Null()
            .When(x => x.VehicleType is VehicleType.SUV or VehicleType.Truck)
            .WithMessage($"Number of doors is not allowed for {VehicleType.SUV}, or {VehicleType.Truck} vehicles.");
    }
}