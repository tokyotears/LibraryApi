namespace web.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next) {
	private readonly RequestDelegate _next = next;

	public async Task Invoke(HttpContext context) {
		try {
			await _next(context);
		} catch (Exception ex) {
			context.Response.StatusCode = 500;
			await context.Response.WriteAsJsonAsync(new {
				error = "internal_server_error",
				message = ex.Message
			});
		}
	}
}
