using web.DTO;

namespace web.Utilities;

public static class MapServiceResults {
	public static IResult ToIResult(Response response) {
		return response.Error switch {
			ErrorType.InvalidInput => Results.BadRequest(new { error = "Invalid input" }),
			ErrorType.UserNotFound => Results.NotFound(new { error = "User not found" }),
			ErrorType.WrongPassword => Results.Unauthorized(),
			ErrorType.UserAlreadyExists => Results.Conflict(new { error = "User already exists" }),
			ErrorType.Forbidden => Results.Forbid(),
			ErrorType.NoUsers => Results.NotFound(new {error = "No users"}),
			ErrorType.LibNotFound => Results.NotFound(new {error = "Library not found"}),
			ErrorType.LibExists => Results.NotFound(new {error = "Library already exists"}),
			_ => Results.StatusCode(StatusCodes.Status500InternalServerError)
		};
	}
}
