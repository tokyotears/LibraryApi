namespace web.DTO;

public record UserAuthRequest(string Name, string Password, string? Role);
