namespace web.Models;

public class Book(string author, string title, Guid libId) {
	public Guid Id { get; set; } = Guid.NewGuid();
	public string Author { get; set; } = author;
	public string Title { get; set; } = title;

	public Guid LibId { get; set; } = libId;
}
