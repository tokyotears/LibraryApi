using web.DTO;
using web.Services;
using web.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace web.Controllers;

[ApiController]
public class UserController(UserService userService, JwtService jwtService) : ControllerBase
{
    [HttpPost("/login")]
    public async Task<IActionResult> Login(UserAuthRequest req)
    {
        var userServiceCall = await userService.Login(req);
        if (!userServiceCall.IsSuccess) return MapServiceResults.ToResult(userServiceCall);
        
        var token = jwtService.CreateJwtToken(userServiceCall.Data!);
        HttpContext.Response.Cookies.Append("jwt_token", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Strict,
        });
        return Ok("Successfully logged in");
    }

    [HttpPost("/register")]
    public async Task<IActionResult> Register(UserAuthRequest req)
    {
        var userServiceCall = await userService.Register(req);
        return userServiceCall.IsSuccess ? Ok("Registered successfully") : MapServiceResults.ToResult(userServiceCall);
    }

    [HttpPost("/logout")]
    public async Task<IActionResult> Logout()
    {
        HttpContext.Response.Cookies.Delete("jwt_token");
        return Ok("Logged out successfully");
    }

    [HttpGet("/users"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUsers()
    {
        var userServiceCall = await userService.GetUsers();
        return userServiceCall.IsSuccess ? Ok(userServiceCall.Data) : MapServiceResults.ToResult(userServiceCall);
    }

    [HttpDelete("/users/me"), Authorize]
    public async Task<IActionResult> DeleteMe()
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdValue, out var parsedId)) return Unauthorized();

        var userServiceCall = await userService.Delete(parsedId);
        if (!userServiceCall.IsSuccess) return MapServiceResults.ToResult(userServiceCall);

        Response.Cookies.Delete("jwt_token");
        return NoContent();
    }

    [HttpDelete("/users/{id:guid}"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
    {
        var userServiceCall = await userService.Delete(id);
        return userServiceCall.IsSuccess ? NoContent() : MapServiceResults.ToResult(userServiceCall);
    }
}