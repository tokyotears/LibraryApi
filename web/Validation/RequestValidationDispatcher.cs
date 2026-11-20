using FluentValidation;
using FluentValidation.Results;
using web.Interfaces;

namespace web.Validation;

public sealed class RequestValidationDispatcher(IServiceProvider services) : IRequestValidationDispatcher
{
    public Task<ValidationResult> ValidateAsync<TRequest>(TRequest request)
    {
        var validator = services.GetService<IValidator<TRequest>>()
                        ?? throw new InvalidOperationException($"No validator registered for {typeof(TRequest).Name}");

        return validator.ValidateAsync(request);
    }
}