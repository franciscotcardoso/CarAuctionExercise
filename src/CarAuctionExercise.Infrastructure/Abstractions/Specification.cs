using System.Linq.Expressions;
using CarAuctionExercise.Infrastructure.Interfaces;

namespace CarAuctionExercise.Infrastructure.Abstractions;

public class Specification<T> : ISpecification<T>
{
    public Expression<Func<T, bool>> Criteria { get; }
    
    public Specification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }
}