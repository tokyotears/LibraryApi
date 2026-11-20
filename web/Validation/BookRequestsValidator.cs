using FluentValidation;
using web.DTO;

namespace web.Validation;

public class BookCreateReqValidator : AbstractValidator<BookCreateRequest> {
    public BookCreateReqValidator() {
        RuleFor(req => req.Author).NotNull().MinimumLength(3).MaximumLength(50);
        RuleFor(req => req.Title).NotNull().MinimumLength(3).MaximumLength(50);
    }
}

public class BookUpdateReqValidator : AbstractValidator<BookUpdateRequest> {
    public BookUpdateReqValidator()
    {
        RuleFor(req => req).Must(req => req.Author is not null || req.Title is not null)
            .WithMessage("At least one field must be provided");
        RuleFor(req => req.Author).NotEmpty().MinimumLength(3).MaximumLength(50);
        RuleFor(req => req.Title).NotEmpty().MinimumLength(3).MaximumLength(50);
    }
}