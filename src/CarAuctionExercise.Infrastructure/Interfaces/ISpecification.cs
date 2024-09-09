using System.Linq.Expressions;

namespace CarAuctionExercise.Infrastructure.Interfaces;

public interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; }
}