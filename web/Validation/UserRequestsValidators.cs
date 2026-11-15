using web.DTO;
using FluentValidation;

namespace web.Validation;

public class UserAuthReqValidator : AbstractValidator<UserAuthRequest> {
    public UserAuthReqValidator() {
        RuleFor(req => req.Name).NotNull().MinimumLength(1);
        RuleFor(req => req.Password).NotNull().MinimumLength(6);
    }
}

public class UserAddReqValidator : AbstractValidator<UserAddRequest> {
    public UserAddReqValidator() {
        RuleFor(req => req.Name).NotNull().MinimumLength(1);
        RuleFor(req => req.Password).NotNull().MinimumLength(6);
        RuleFor(req => req.Role).NotNull().IsInEnum();
    }
}