namespace web.Models;

public class Library(Guid id, string name, string? desc) {
	public Guid Id { get; set; } = id;
	public string Name { get; set; } = name;
	public string? Desc { get; set; } = desc;
	public DateTime CreatedAd { get; } = DateTime.Now;
}
