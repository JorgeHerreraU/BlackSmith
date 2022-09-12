namespace BlackSmith.Business.Interfaces;

public interface IComplexValidator<T>
{
    Task ValidateUpdateAndThrowAsync(T t);
    Task ValidateCreateAndThrowAsync(T t);
}
