using web.Enums;

namespace web.Models;

public class User(string name, Guid id, string hashedPassword, Roles role) {
	public Guid Id { get; set; } = id;
	public string Name { get; set; } = name;
	public string HashedPassword { get; set; } = hashedPassword;
	public Roles Role { get; set; } = role;
}
