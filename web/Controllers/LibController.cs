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

    [HttpGet("/libraries/me"), Authorize]
    public async Task<IActionResult> GetMyLibrary()
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdValue, out var parsedId)) return Unauthorized();

        var libServiceCall = await libService.GetLibrary(parsedId);
        return libServiceCall.IsSuccess ? Ok(libServiceCall.Data) : MapServiceResults.ToResult(libServiceCall);
    }

    [HttpGet("/libraries/me/books"), Authorize]
    public async Task<IActionResult> GetMyBooks()
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdValue, out var parsedId)) return Unauthorized(); 
        
        var libServiceCall = await libService.GetBooks(parsedId);
        return libServiceCall.IsSuccess ? Ok(libServiceCall.Data) : MapServiceResults.ToResult(libServiceCall);
    }

    [HttpPost("/libraries/add"), Authorize]
    public async Task<IActionResult> AddLibrary(LibAddRequest req)
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdValue, out var parsedId)) return Unauthorized(); 
        
        var libServiceCall = await libService.AddLibrary(parsedId, req);
        return libServiceCall.IsSuccess ? Ok() : MapServiceResults.ToResult(libServiceCall);
    }

    [HttpPost("/libraries/me/addbook"), Authorize]
    public async Task<IActionResult> AddBook(BookAddRequest req)
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdValue, out var parsedId)) return Unauthorized(); 
        
        var libServiceCall = await libService.AddBook(parsedId, req);
        return libServiceCall.IsSuccess ? Ok() : MapServiceResults.ToResult(libServiceCall); 
    }
    
    [HttpPost("/libraries/me/edit"), Authorize]
    public async Task<IActionResult> UpdateLibrary(LibAddRequest req)
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdValue, out var parsedId)) return Unauthorized(); 
        
        var libServiceCall = await libService.UpdateLibrary(parsedId, req);
        return libServiceCall.IsSuccess ? Ok() : MapServiceResults.ToResult(libServiceCall); 
    }
    
    [HttpDelete("/libraries/me/books/{id:guid}"), Authorize]
    public async Task<IActionResult> DeleteBook([FromRoute] Guid bookId)
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdValue, out var parsedId)) return Unauthorized(); 
        
        var libServiceCall = await libService.DeleteBook(parsedId, bookId);
        return libServiceCall.IsSuccess ? NoContent() : MapServiceResults.ToResult(libServiceCall); 
    } 
}