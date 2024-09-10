namespace CarAuctionExercise.Api.Extensions;

using System.Diagnostics.CodeAnalysis;
using CarAuctionExercise.Infrastructure.Data;
using CarAuctionExercise.Infrastructure.Interfaces;

[ExcludeFromCodeCoverage]
public static class AddRepositoryExtension
{
    public static void AddRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton(typeof(IServiceRepository<>), typeof(ServiceRepository<>));
    }
}