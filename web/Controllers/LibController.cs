using web.DTO;
using web.Services;
using web.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace web.Controllers;

[ApiController]
public class LibController(LibService libService) : ControllerBase
{
    [HttpGet("/libraries/all"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllLibraries()
    {
        var libServiceCall = await libService.GetAll();
        return libServiceCall.IsSuccess ? Ok(libServiceCall.Data) : MapServiceResults.ToResult(libServiceCall);
    }

    [HttpGet("/libraries"), Authorize]
    public async Task<IActionResult> GetMyLibraries()
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdValue, out var parsedId)) return Unauthorized();

        var libServiceCall = await libService.GetMyLibraries(parsedId);
        if (!libServiceCall.IsSuccess) return MapServiceResults.ToResult(libServiceCall);

        return Ok(libServiceCall.Data);
    }

    [HttpGet("/libraries/{libId:guid}"), Authorize]
    public async Task<IActionResult> GetMyLibrary([FromRoute] Guid libId)
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdValue, out var parsedId)) return Unauthorized();

        var libServiceCall = await libService.GetLibrary(parsedId, libId);
        return libServiceCall.IsSuccess ? Ok(libServiceCall.Data) : MapServiceResults.ToResult(libServiceCall);
    }

    [HttpGet("/libraries/{libId:guid}/books"), Authorize]
    public async Task<IActionResult> GetMyBooks([FromRoute] Guid libId)
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdValue, out var parsedId)) return Unauthorized(); 
        
        var libServiceCall = await libService.GetBooks(parsedId, libId);
        return libServiceCall.IsSuccess ? Ok(libServiceCall.Data) : MapServiceResults.ToResult(libServiceCall);
    }

    [HttpPost("/libraries/add"), Authorize]
    public async Task<IActionResult> AddLibrary(LibCreateRequest req)
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdValue, out var parsedId)) return Unauthorized(); 
        
        var libServiceCall = await libService.AddLibrary(parsedId, req);
        return libServiceCall.IsSuccess ? Ok() : MapServiceResults.ToResult(libServiceCall);
    }

    [HttpPost("/libraries/{libId:guid}/addbook"), Authorize]
    public async Task<IActionResult> AddBook([FromRoute] Guid libId, BookCreateRequest req)
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdValue, out var parsedId)) return Unauthorized(); 
        
        var libServiceCall = await libService.AddBook(parsedId, libId, req);
        return libServiceCall.IsSuccess ? Ok() : MapServiceResults.ToResult(libServiceCall); 
    }
    
    [HttpPatch("/libraries/{libId:guid}/edit"), Authorize]
    public async Task<IActionResult> UpdateLibrary([FromRoute] Guid libId, LibUpdateRequest req)
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdValue, out _)) return Unauthorized(); 
        
        var libServiceCall = await libService.UpdateLibrary(libId, req);
        return libServiceCall.IsSuccess ? Ok() : MapServiceResults.ToResult(libServiceCall); 
    }
    
    [HttpDelete("/libraries/{libId:guid}/books/{bookid:guid}"), Authorize]
    public async Task<IActionResult> DeleteBook([FromRoute] Guid libId, [FromRoute] Guid bookId)
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdValue, out var parsedId)) return Unauthorized(); 
        
        var libServiceCall = await libService.DeleteBook(parsedId, libId, bookId);
        return libServiceCall.IsSuccess ? NoContent() : MapServiceResults.ToResult(libServiceCall); 
    }

    [HttpDelete("/libraries/{libId:guid}"), Authorize]
    public async Task<IActionResult> DeleteLibrary([FromRoute] Guid libId)
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdValue, out _)) return Unauthorized();

        var libServiceCall = await libService.DeleteLibrary(libId);

        return libServiceCall.IsSuccess ? Ok() : MapServiceResults.ToResult(libServiceCall);
    }
}