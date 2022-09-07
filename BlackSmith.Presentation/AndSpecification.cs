using BlackSmith.Presentation.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace BlackSmith.Presentation;

public class AndSpecification<T> : ISpecification<T>
{
    private readonly IEnumerable<ISpecification<T>> _specifications;

    public AndSpecification(IEnumerable<ISpecification<T>> specifications)
    {
        _specifications = specifications;
    }
    public bool IsSatisfied(T t)
    {
        return _specifications.All(specification => specification.IsSatisfied(t));
    }
}
