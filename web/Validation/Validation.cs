using FluentValidation;
using web.DTO;

namespace web.Validation;

public class UserAuthReqValidator : AbstractValidator<UserAuthRequest> {
	public UserAuthReqValidator() {
		RuleFor(req => req.Name).NotNull().MinimumLength(3);
		RuleFor(req => req.Password).NotNull().MinimumLength(6);
	}
}

public class LibAddReqValidator : AbstractValidator<LibAddRequest> {
	public LibAddReqValidator() {
		RuleFor(req => req.Name).NotNull().MinimumLength(3);
		RuleFor(req => req.Desc).MaximumLength(100);
	}
}

public class BookAddReqValidator : AbstractValidator<BookAddRequest> {
	public BookAddReqValidator() {
		RuleFor(req => req.Author).NotNull().MinimumLength(3);
		RuleFor(req => req.Title).NotNull().MinimumLength(3);
	}
}
