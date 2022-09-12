namespace BlackSmith.Business.Interfaces;

public interface IUpdateComplexValidation<T>
{
    Task ValidateAsync(T t);
}
