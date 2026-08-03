using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using web.Models;

namespace web.DataConfigs;

public class BookConfiguration : IEntityTypeConfiguration<Book> {
	public void Configure (EntityTypeBuilder<Book> entity) {
		entity.HasKey(b => b.Id);
		entity.HasIndex(b => new { b.Author, b.Title , b.LibId}).IsUnique();
		entity.Property(b => b.Author).IsRequired();
		entity.Property(b => b.Title).IsRequired();
	}
}
