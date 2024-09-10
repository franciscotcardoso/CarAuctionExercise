namespace CarAuctionExercise.Infrastructure.Interfaces;

using System.Linq.Expressions;

public interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; }
}