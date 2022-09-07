namespace BlackSmith.Presentation.Interfaces;

public interface ISpecification<T>
{
    bool IsSatisfied(T t);
}
