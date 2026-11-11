using web.Enums;

namespace web.DTO;

public record UserAuthRequest(string Name, string Password, Roles Role);
