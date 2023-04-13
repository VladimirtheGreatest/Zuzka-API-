namespace Zuzka.Services.Contracts
{
    public interface IValidationService
    {
        ValidationResult Validate<T>(T obj);
    }
}