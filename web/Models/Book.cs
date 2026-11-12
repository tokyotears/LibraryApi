namespace web.Models;

public class Book(Guid id, string author, string title, Guid libId) {
	public Guid Id { get; set; } = id;
	public string Author { get; set; } = author;
	public string Title { get; set; } = title;

	public Guid LibId { get; set; } = libId;
}
