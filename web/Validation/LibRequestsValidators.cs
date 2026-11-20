using FluentValidation;
using web.DTO;

namespace web.Validation;

public class LibCreateReqValidator : AbstractValidator<LibCreateRequest> {
    public LibCreateReqValidator() {
        RuleFor(req => req.Name).NotNull().MinimumLength(3).MaximumLength(50);
        RuleFor(req => req.Desc).MaximumLength(100);
    }
}

public class LibUpdateReqValidator : AbstractValidator<LibUpdateRequest> {
    public LibUpdateReqValidator()
    {
        RuleFor(req => req).Must(req => req.Name is not null || req.Desc is not null)
            .WithMessage("At least on field must be provided");
        RuleFor(req => req.Name).NotEmpty().MinimumLength(3).MaximumLength(50).When(req => req.Name is not null);
        RuleFor(req => req.Desc).NotEmpty().MaximumLength(200).When(req => req.Desc is not null);
    }
}