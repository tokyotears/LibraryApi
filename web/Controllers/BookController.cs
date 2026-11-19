using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using web.DTO;
using web.Services;
using web.Utilities;

namespace web.Controllers;

[ApiController]
public class BookController(BookService bookService) : ControllerBase
{
    [HttpGet("/libraries/{libId:guid}/books"), Authorize]
    public async Task<IActionResult> GetMyBooks([FromRoute] Guid libId)
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdValue, out var parsedId)) return Unauthorized(); 
        
        var bookServiceCall = await bookService.GetBooks(parsedId, libId);
        return bookServiceCall.IsSuccess ? Ok(bookServiceCall.Data) : MapServiceResults.ToResult(bookServiceCall);
    } 
    
    [HttpPost("/libraries/{libId:guid}"), Authorize]
    public async Task<IActionResult> AddBook([FromRoute] Guid libId, BookCreateRequest req)
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdValue, out var parsedId)) return Unauthorized(); 
        
        var bookServiceCall = await bookService.AddBook(parsedId, libId, req);
        return bookServiceCall.IsSuccess ? Ok() : MapServiceResults.ToResult(bookServiceCall); 
    }
    
    [HttpDelete("/libraries/{libId:guid}/books/{bookId:guid}"), Authorize]
    public async Task<IActionResult> DeleteBook([FromRoute] Guid libId, [FromRoute] Guid bookId)
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdValue, out var parsedId)) return Unauthorized(); 
        
        var bookServiceCall = await bookService.DeleteBook(parsedId, libId, bookId);
        return bookServiceCall.IsSuccess ? NoContent() : MapServiceResults.ToResult(bookServiceCall); 
    }

    [HttpPatch("/libraries/{libId:guid}/books/{bookId:guid}"), Authorize]
    public async Task<IActionResult> UpdateBook([FromRoute] Guid libId, [FromRoute] Guid bookId, BookUpdateRequest req)
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdValue, out var parsedId)) return Unauthorized();

        var bookServiceCall = await bookService.UpdateBook(parsedId, libId, bookId, req);

        if (!bookServiceCall.IsSuccess) return MapServiceResults.ToResult(bookServiceCall);

        return Ok();
    }
}