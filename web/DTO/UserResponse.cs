using web.Enums;

namespace web.DTO;

public record UserResponse(Guid Id, string Name, Roles Role);