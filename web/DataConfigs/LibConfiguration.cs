using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using web.Models;

namespace web.DataConfigs;

public class LibConfiguration : IEntityTypeConfiguration<Library> {
	public void Configure (EntityTypeBuilder<Library> entity) {
		entity.HasKey(l => l.Id);
		entity.Property(l => l.Name).IsRequired();
		entity.HasMany<Book>().WithOne().HasForeignKey(b => b.LibId);
	}
}