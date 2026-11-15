using web.Enums;

namespace web.DTO;

public record UserAddRequest(string Name, string Password, Roles Role);
