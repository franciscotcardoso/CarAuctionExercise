using System.Diagnostics.CodeAnalysis;
using CarAuctionExercise.Application.Interfaces;
using CarAuctionExercise.Application.Services;

namespace CarAuctionExercise.Api.Extensions;

[ExcludeFromCodeCoverage]
public static class AddServicesExtension
{
    public static void AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IVehiclesManagementService, VehiclesManagementService>();
        serviceCollection.AddScoped<IAuctionsManagementService, AuctionsManagementService>();
    }
}