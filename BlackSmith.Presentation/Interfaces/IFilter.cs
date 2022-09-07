using System.Collections.Generic;

namespace BlackSmith.Presentation.Interfaces;

public interface IFilter<T>
{
    IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> specification);
}
