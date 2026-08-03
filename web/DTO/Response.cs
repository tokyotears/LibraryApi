namespace web.DTO;

public enum ErrorType {
	InvalidInput,
	UserNotFound,
	WrongPassword,
	UserAlreadyExists,
	NoUsers,
	Forbidden,
	LibNotFound,
	LibExists
}

public class Response {
	public bool IsSuccess => Error is null;
	public ErrorType? Error { get; init; }
}

public class Response<T> : Response {
	public T? Data { get; init; }
}
