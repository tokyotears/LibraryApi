using web.Enums;

namespace web.DTO;

public class Response {
	public bool IsSuccess => Error is null;
	public ErrorType? Error { get; init; }
	public List<string> Errors { get; set; } = [];
}

public class Response<T> : Response {
	public T? Data { get; init; }
}
