using web.Utilities;
using web.DTO;
using web.Interfaces;
using web.Validation;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace web.Endpoints;

public static class LibEndpoints {
	private static bool Authorize(ClaimsPrincipal claims, Guid? id = null) {
		var userId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		if (userId is null) return false;
		if (id != null && userId != id.ToString()) return false;
		return true;
	}

	public static void MapLibEndpoints(this WebApplication app) {
		app.MapGet("/libraries", [Authorize] async (ILibRepository libRepository, ILibService libService, ClaimsPrincipal claims) => {
			if (claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value != "Admin") return Results.Forbid();
			var temp = await libService.GetAll(libRepository);
			var res = MapServiceResults.ToIResult(temp);
			if (temp.IsSuccess) return Results.Ok(temp.Data);
			else return res;
		});

		app.MapGet("/libraries/{id}", [Authorize] async (ILibRepository libRepository, ILibService libService, ClaimsPrincipal claims, Guid id) => {
			if (!Authorize(claims, id)) return Results.Forbid();
			var temp = await libService.GetLibrary(libRepository, id);
			var res = MapServiceResults.ToIResult(temp);
			if (temp.IsSuccess) return Results.Ok(temp.Data);
			else return res;
		});

		app.MapGet("/libraries/{id}/books", [Authorize] async (ILibRepository libRepository, ILibService libService, ClaimsPrincipal claims, Guid id) => {
			if (!Authorize(claims, id)) return Results.Forbid();
			var temp = await libService.GetBooks(libRepository, id);
			if (temp.IsSuccess) return Results.Ok(temp.Data);
			else return MapServiceResults.ToIResult(temp);
		});

		app.MapPost("/libraries/{id}/add", [Authorize] async (ILibRepository libRepository, ILibService libService, LibAddReqValidator validator, ClaimsPrincipal claims, Guid id, LibAddRequest req) => {
			if (!Authorize(claims, id)) return Results.Forbid();
			var temp = await libService.AddLibrary(libRepository, validator, id, req);
			if (temp.IsSuccess) return Results.Ok();
			else return MapServiceResults.ToIResult(temp);
		});

		app.MapPost("/libraries/{id}/addbook", [Authorize] async (ILibRepository libRepository, ILibService libService, BookAddReqValidator validator, ClaimsPrincipal claims, Guid id, BookAddRequest book) => {
			if (!Authorize(claims, id)) return Results.Forbid();
			var temp = await libService.AddBook(libRepository, validator, id, book);
			if (temp.IsSuccess) return Results.Ok();
			else return MapServiceResults.ToIResult(temp);
		});

		app.MapDelete("/libraries/{id}/books/{bookId}", [Authorize] async (ILibRepository libRepository, ILibService libService, ClaimsPrincipal claims, Guid id, Guid bookId) => {
			if (!Authorize(claims, id)) return Results.Forbid();
			var temp = await libService.DeleteBook(libRepository, id, bookId);
			if (temp.IsSuccess) return Results.Ok();
			else return MapServiceResults.ToIResult(temp);
		});
	}
}
