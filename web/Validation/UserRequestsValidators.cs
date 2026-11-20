using web.DTO;
using FluentValidation;

namespace web.Validation;

public class UserAuthReqValidator : AbstractValidator<UserAuthRequest> {
    public UserAuthReqValidator() {
        RuleFor(req => req.Name).NotNull().MinimumLength(1).MaximumLength(50);
        RuleFor(req => req.Password).NotNull().MinimumLength(6).MaximumLength(50);
    }
}

public class UserCreateReqValidator : AbstractValidator<UserCreateRequest> {
    public UserCreateReqValidator() {
        RuleFor(req => req.Name).NotNull().MinimumLength(1).MaximumLength(50);
        RuleFor(req => req.Password).NotNull().MinimumLength(6).MaximumLength(50);
        RuleFor(req => req.Role).NotNull().IsInEnum();
    }
}

public class UserUpdateReqValidator : AbstractValidator<UserUpdateRequest> {
    public UserUpdateReqValidator()
    {
        RuleFor(req => req).Must(req => req.Name is not null || req.Password is not null).WithMessage("At least one field must be provided");
        RuleFor(req => req.Name).NotEmpty().MinimumLength(1).MaximumLength(50).When(req => req.Name is not null);
        RuleFor(req => req.Password).NotEmpty().MinimumLength(6).MaximumLength(50).When(req => req.Password is not null);
    }
}