using web.Enums;

namespace web.DTO;

public record UserCreateRequest(string Name, string Password, Roles Role);
