using FluentValidation.Results;

namespace web.Interfaces;

public interface IRequestValidationDispatcher
{
    Task<ValidationResult> ValidateAsync<TRequest>(TRequest request);
}