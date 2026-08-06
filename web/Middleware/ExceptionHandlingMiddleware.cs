namespace web.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger) {
	private readonly RequestDelegate _next = next;
	private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

	public async Task Invoke(HttpContext context) {
		try {
			await _next(context);
		} catch (Exception ex) {
			context.Response.StatusCode = 500;
			_logger.LogError(ex, "An unhandled exception occurred while processing the request.");

			await context.Response.WriteAsJsonAsync(new {
				error = "internal_server_error",
				message = ex.Message
			});
		}
	}
}
