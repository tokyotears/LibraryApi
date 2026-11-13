using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using web.Models;

namespace web.DataConfigs;

public class UserConfiguration : IEntityTypeConfiguration<User> {
	public void Configure (EntityTypeBuilder<User> entity) {
		entity.HasKey(u => u.Id);
		entity.HasIndex(u => u.Name).IsUnique();
		entity.Property(u => u.Name).IsRequired();
		entity.Property(u => u.HashedPassword).IsRequired();
		entity.Property(u => u.Role).HasConversion<string>().IsRequired();
		entity.HasMany<Library>().WithOne().HasForeignKey(l => l.UserId);
	}
}
