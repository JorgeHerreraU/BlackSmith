namespace BlackSmith.Business.Interfaces;

public interface ICreateComplexValidation<T>
{
    Task ValidateAsync(T t);
}
