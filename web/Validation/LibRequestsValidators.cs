using FluentValidation;
using web.DTO;

namespace web.Validation;

public class LibAddReqValidator : AbstractValidator<LibCreateRequest> {
    public LibAddReqValidator() {
        RuleFor(req => req.Name).NotNull().MinimumLength(3);
        RuleFor(req => req.Desc).MaximumLength(100);
    }
}

public class BookAddReqValidator : AbstractValidator<BookCreateRequest> {
    public BookAddReqValidator() {
        RuleFor(req => req.Author).NotNull().MinimumLength(3);
        RuleFor(req => req.Title).NotNull().MinimumLength(3);
    }
}