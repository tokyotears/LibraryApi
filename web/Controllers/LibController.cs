using web.Interfaces;
using web.DTO;
using web.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace web.Controllers;

[ApiController]
public class LibController(ILibService libService) : ControllerBase
{
    [HttpGet("/libraries"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetLibraries()
    {
        var libServiceCall = await libService.GetAll();
        return libServiceCall.IsSuccess ? Ok(libServiceCall.Data) : MapServiceResults.ToResult(libServiceCall);
    }

    [HttpGet("/libraries/me/{id:guid}"), Authorize]
    public async Task<IActionResult> GetMyLibrary([FromRoute] Guid id)
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdValue, out _)) return Unauthorized();

        var libServiceCall = await libService.GetLibrary(id);
        return libServiceCall.IsSuccess ? Ok(libServiceCall.Data) : MapServiceResults.ToResult(libServiceCall);
    }

    [HttpGet("/libraries/me/{id:guid}/books"), Authorize]
    public async Task<IActionResult> GetMyBooks([FromRoute] Guid id)
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdValue, out _)) return Unauthorized(); 
        
        var libServiceCall = await libService.GetBooks(id);
        return libServiceCall.IsSuccess ? Ok(libServiceCall.Data) : MapServiceResults.ToResult(libServiceCall);
    }

    [HttpPost("/libraries/me/add"), Authorize]
    public async Task<IActionResult> AddLibrary(LibAddRequest req)
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdValue, out var parsedId)) return Unauthorized(); 
        
        var libServiceCall = await libService.AddLibrary(parsedId, req);
        return libServiceCall.IsSuccess ? Ok() : MapServiceResults.ToResult(libServiceCall);
    }

    [HttpPost("/libraries/me/{id:guid}/addbook"), Authorize]
    public async Task<IActionResult> AddBook([FromRoute] Guid id, BookAddRequest req)
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdValue, out _)) return Unauthorized(); 
        
        var libServiceCall = await libService.AddBook(id, req);
        return libServiceCall.IsSuccess ? Ok() : MapServiceResults.ToResult(libServiceCall); 
    }
    
    [HttpPost("/libraries/me/{id:guid}/edit"), Authorize]
    public async Task<IActionResult> UpdateLibrary([FromRoute] Guid id, LibAddRequest req)
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdValue, out _)) return Unauthorized(); 
        
        var libServiceCall = await libService.UpdateLibrary(id, req);
        return libServiceCall.IsSuccess ? Ok() : MapServiceResults.ToResult(libServiceCall); 
    }
    
    [HttpDelete("/libraries/me/{id:guid}/books/{bookid:guid}"), Authorize]
    public async Task<IActionResult> DeleteBook([FromRoute] Guid id, [FromRoute] Guid bookId)
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdValue, out _)) return Unauthorized(); 
        
        var libServiceCall = await libService.DeleteBook(id, bookId);
        return libServiceCall.IsSuccess ? NoContent() : MapServiceResults.ToResult(libServiceCall); 
    } 
}