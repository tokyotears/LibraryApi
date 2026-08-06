namespace web.Models;

public enum Roles {
	User,
	Admin
}

public class User(string name, Guid id, string hashedPassword, string? role) {
	public Guid Id { get; set; } = id;
	public string Name { get; set; } = name;
	public string HashedPassword { get; set; } = hashedPassword;
	public string? Role { get; set; } = role switch {
		"Admin" => Roles.Admin.ToString(),
		"User" => Roles.User.ToString(),
		_ => throw new ArgumentException("Invalid role")
	};
}
