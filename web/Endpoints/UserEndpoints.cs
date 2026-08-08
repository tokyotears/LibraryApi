using web.Services;
using web.Utilities;
using web.DTO;
using web.Interfaces;
using web.Models;
using System.Security.Claims;
using web.Validation;

namespace web.Endpoints;

public static class UserEndpoints {
	public static void MapUserEndpoints(this WebApplication app) {
		app.MapPost("/login", async (IUserRepository userRepository, IUserService userService, UserAuthReqValidator validator, UserAuthRequest req, HttpContext context, JwtService jwtService) => {
			var temp = await userService.Login(userRepository, validator, req);
			if (temp.IsSuccess == true) {
				var token = jwtService.CreateJwtToken(temp.Data!);
				context.Response.Cookies.Append("jwt_token", token, new CookieOptions {
					HttpOnly = true,
					Secure = false,
					SameSite = SameSiteMode.Strict,
				});
				return Results.Ok(new { message = "Login successful" });
			}
			return MapServiceResults.ToIResult(temp);
		});
		app.MapPost("/register", async (IUserRepository userRepository, IUserService userService, UserAuthReqValidator validator, UserAuthRequest req) => {
			var temp = await userService.Register(userRepository, validator, req);
			if (temp.IsSuccess == true) {
				return Results.Ok(new { message = "Registration successful" });
			}
			return MapServiceResults.ToIResult(temp);
		});
		app.MapPost("/logout", (HttpContext context) => {
			context.Response.Cookies.Delete("jwt_token");
			return Results.Ok(new { message = "Logged out successfully" });
		});
		app.MapGet("/users", async (IUserRepository userRepository, IUserService userService, ClaimsPrincipal claims) => {
			if (claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value != Roles.Admin.ToString()) {
				return Results.Forbid();
			}
			var temp = await userService.GetUsers(userRepository);
			if (temp.IsSuccess == true) {
				return Results.Ok(temp.Data);
			}
			return MapServiceResults.ToIResult(temp);
		});
		app.MapDelete("/users/{id}", async (IUserRepository userRepository, IUserService userService, ClaimsPrincipal claims, HttpContext context, Guid id) => {
			bool isAdmin = false;
			if (claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value != id.ToString()) {
				if (claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value != Roles.Admin.ToString()) {
					return Results.Forbid();
				} else isAdmin = true;
			}
			var temp = await userService.Delete(userRepository, id);
			if (temp.IsSuccess == true) {
				if (!isAdmin) context.Response.Cookies.Delete("jwt_token");
				return Results.Ok(new { message = "User deleted successfully" });
			}
			return MapServiceResults.ToIResult(temp);
		});
	}
}
