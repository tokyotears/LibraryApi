namespace web.Models;

public class Library(Guid id, Guid userId, string name, string? desc, DateTime createdAt) {
	public Guid Id { get; init; } = id;
	public Guid UserId { get; init; } = userId;
	public string Name { get; set; } = name;
	public string? Desc { get; set; } = desc;
	public DateTime CreatedAt { get; } = createdAt;
}