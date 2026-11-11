using web.DTO;
using web.Enums;
using Microsoft.AspNetCore.Mvc;

namespace web.Utilities;

public static class MapServiceResults {
	public static IActionResult ToResult(Response response) {
		return response.Error switch {
			ErrorType.InvalidInput => new BadRequestObjectResult(new {error = "Invalid input"}),
			ErrorType.UserNotFound => new NotFoundObjectResult(new { error = "User not found" }),
			ErrorType.WrongPassword => new UnauthorizedResult(),
			ErrorType.UserAlreadyExists => new ConflictObjectResult(new { error = "User already exists" }),
			ErrorType.Forbidden => new ForbidResult(),
			ErrorType.NoUsers => new NotFoundObjectResult(new {error = "No users"}),
			ErrorType.LibNotFound => new NotFoundObjectResult(new {error = "Library not found"}),
			ErrorType.LibExists => new BadRequestObjectResult(new {error = "Library already exists"}),
			_ => new ObjectResult("Internal server error") {StatusCode = 500}
		};
	}
}
